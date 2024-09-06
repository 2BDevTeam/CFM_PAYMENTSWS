using CFM_PAYMENTSWS.Domains.Interface;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.Extensions;
using CFM_PAYMENTSWS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CFM_PAYMENTSWS.DTOs;

namespace CFM_PAYMENTSWS.Persistence.Repositories
{
    public class GenericRepository<TContext> : IGenericRepository<TContext> where TContext : DbContext
    {

        private readonly TContext appDbContext;

        private readonly ConversionExtension conversionExtension=new ConversionExtension();



        public static List<T> PaginateQueryResult<T>(IQueryable<T> query, int? pageIndex, int? pageSize)
        {
            List<T> resultPaginated;
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                resultPaginated = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
            }
            else
            {
                resultPaginated = query.ToList();
            }

            return resultPaginated;
        }


        public GenericRepository(TContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public void SaveChanges()
        {
            appDbContext.SaveChanges();
        }

        public void BulkOverWrite<T>(List<List<T>> entityLists) where T : class
        {
            var transaction = appDbContext.Database.BeginTransaction();
            try
            {
                foreach (var list in entityLists)
                {
                    appDbContext.RemoveRange(list);
                }

                appDbContext.SaveChanges(); // Remove data*/

                foreach (var list in entityLists)
                {
                    appDbContext.AddRange(list);
                }

                appDbContext.SaveChanges(); // Add new data

                transaction.Commit();
               

            }
          
            catch (Exception ex)
            {

                Debug.Print("ROLLING BACK BulkOverWrite ");
                transaction.Rollback();                
                throw new Exception("BULKOVERRITE EXEPTION: " + ex.Message + "INNER GenericRepository Repository: " + ex.InnerException + " Stack BulkOverWrite Repository: " + ex.StackTrace);

            }
        }

        public void BulkUpsertEntity<T>(List<T> entities, List<string> keysToExclude, bool saveChanges) where T : class
        {

            foreach(var entity in entities)
            {
                var conditions = new List<KeyValuePair<string, object>>();

                var keyToExclude=keysToExclude.FirstOrDefault();

                if(keyToExclude != null)
                {
                  conditions.Add(new KeyValuePair<string, object>(keyToExclude, entity.GetValObjDy(keyToExclude)));

                }


                UpsertEntity(entity, keysToExclude, conditions, saveChanges);
            }

        }

        public void UpsertEntity<T>(T entity, List<string> keysToExclude, List<KeyValuePair<string, object>> conditions, bool saveChanges) where T : class
        {
            var entityType = typeof(T);

            List<PropertyInfo> keysToExcludeProperties= new List<PropertyInfo>();


           
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new IgnoreNullAndDefaultValuesContractResolver(),
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(entity, settings);
            //Debug.Print(json);
            var entityObject = JObject.Parse(json);

            // Extract the property names into a string array
            var modifiedProperties = entity.GetAssignedProperties();

            //foreach (var prop in modifiedProperties)
            //{
            //    Debug.Print($"ASSIGNEDSSSS  {prop}");
            //}


            //foreach (var property in modifiedProperties)
            //{
            //    Debug.Print($" EXTRAIDO  {property}");
            //}


            foreach (var keyToExclude in keysToExclude)
            {
                var keyToExcludeProperty = entityType.GetProperty(keyToExclude);

                if (keyToExcludeProperty == null)
                {
                    throw new InvalidOperationException($"Entity does not have a property named '{keyToExclude}'.");
                }

                keysToExcludeProperties.Add(keyToExcludeProperty);
            }


            





            // Add more conditions as needed

            var parameter = Expression.Parameter(entityType, "e");
            Expression combinedCondition = null;

            foreach (var condition in conditions)
            {
                var keyProperty = entityType.GetProperty(condition.Key);
                if (keyProperty == null)
                {
                    throw new InvalidOperationException($"Entity does not have a property named '{keyProperty}'.");
                }
                var property = entityType.GetProperty(condition.Key);
                var propertyValue = condition.Value;
                var propertyExpression = Expression.Property(parameter, property);
                var constant = Expression.Constant(propertyValue);
                var equalExpression = Expression.Equal(propertyExpression, constant);

                if (combinedCondition == null)
                {
                    combinedCondition = equalExpression;
                }
                else
                {
                    combinedCondition = Expression.AndAlso(combinedCondition, equalExpression);
                }
            }

            var lambda = Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);

            Debug.Print($"Combined {combinedCondition}");
            // var existingEntity = appDbContext.Set<T>().Where(lambda).FirstOrDefault();

            var existingEntity = appDbContext.Set<T>().Where(lambda).FirstOrDefault();







            if (existingEntity != null)
            {
                Debug.Print($"EXISTING ENTITY NOT NULL  ");
                foreach (var property in entityType.GetProperties())
                {
                    //  Debug.Print($" ENTITY {property.Name.ToString()}");

                  

                    if (IsPropertyInList(property,keysToExcludeProperties)==false)
                    {

                        var propertyName= property.Name.ToString();


                        if (modifiedProperties.Contains(propertyName))
                        {


                            //Debug.Print($"THE PROPERTY {propertyName} was set");
                            //    Debug.Print($" ENTITY NOT EXCLUDE {property.Name.ToString()}");
                            var newValue = property.GetValue(entity);

                            if (newValue != null)
                            {
                                var finalValue = newValue;
                                if (property.PropertyType == typeof(DateTime))
                                {

                                    finalValue = conversionExtension.ParseToDate(newValue);
                                }



                                property.SetValue(existingEntity, finalValue);

                               

                            }
                        }
                   


                    }
                }

                appDbContext.Entry(existingEntity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                appDbContext.Entry(existingEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                Debug.Print("New Entityy");

                // var entitytoAdd = ;
                entity.AssignDefaultValues();
                appDbContext.Set<T>().Add(entity);
            }

            if (saveChanges)
            {
                appDbContext.SaveChanges();

            }
        }

        public void Update<T>(T entity) where T : class
        {
            appDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Add<T>(T entity) where T : class
        {
            entity.AssignDefaultEntityValues();
            appDbContext.Add(entity);
        }

        static bool IsPropertyInList(PropertyInfo searchProperty, List<PropertyInfo> propertyList)
        {
            return propertyList.Where(propert=>propert.Name == searchProperty.Name).Any();
        }

        public  bool IsPropertySet<T>(T obj, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null) return false;

            var value = property.GetValue(obj);
            var defaultValue = Activator.CreateInstance(property.PropertyType);
            return !Equals(value, defaultValue);
        }
        public void UpsertEntity2<T>(T entity,string keyToExclude, List<KeyValuePair<string, object>> conditions, bool saveChanges) where T : class
        {
            var entityType = typeof(T);
           

            var keyToExcludeProperty= entityType.GetProperty(keyToExclude);

            if(keyToExcludeProperty==null)
            {
                throw new InvalidOperationException($"Entity does not have a property named '{keyToExcludeProperty}'.");
            }

            
            

          
            // Add more conditions as needed

            var parameter = Expression.Parameter(entityType, "e");
            Expression combinedCondition = null;

            foreach (var condition in conditions)
            {
                var keyProperty = entityType.GetProperty(condition.Key);
                if (keyProperty == null)
                {
                    throw new InvalidOperationException($"Entity does not have a property named '{keyProperty}'.");
                }
                var property = entityType.GetProperty(condition.Key);
                var propertyValue = condition.Value;
                var propertyExpression = Expression.Property(parameter, property);
                var constant = Expression.Constant(propertyValue);
                var equalExpression = Expression.Equal(propertyExpression, constant);

                if (combinedCondition == null)
                {
                    combinedCondition = equalExpression;
                }
                else
                {
                    combinedCondition = Expression.AndAlso(combinedCondition, equalExpression);
                }
            }

            var lambda = Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);

            Debug.Print($"Combined {combinedCondition}");
           // var existingEntity = appDbContext.Set<T>().Where(lambda).FirstOrDefault();

            var existingEntity = appDbContext.Set<T>().Where(lambda).FirstOrDefault();

            





            if (existingEntity != null)
            {
                Debug.Print($"EXISTING ENTITY NOT NULL  ");
                foreach (var property in entityType.GetProperties())
                {
                    if (property != keyToExcludeProperty)
                    {
                        var newValue = property.GetValue(entity);

                       // Debug.Print($"property.Name {property.Name}: {newValue}");
                        

                        if(newValue != null)
                        {
                             var finalValue=newValue;
                              if (property.PropertyType == typeof(DateTime))
                              {

                                finalValue= conversionExtension.ParseToDate(newValue);
                              }
                            

                            property.SetValue(existingEntity, finalValue);

                        }

                        
                    }
                }

                appDbContext.Entry(existingEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                Debug.Print("New Entity");
                appDbContext.Set<T>().Add(entity);
            }

            if (saveChanges)
            {
            appDbContext.SaveChanges();

            }
        }

        public void BulkAdd<T>(IEnumerable<T> entityList) where T : class
        {
           appDbContext.AddRange(entityList);
        }

        public void BulkUpdate<T>(IEnumerable<T> entityList) where T : class
        {
            appDbContext.UpdateRange(entityList);
        }

        public void BulkDelete<T>(IEnumerable<T> entityList) where T : class
        {
           appDbContext.RemoveRange(entityList);
        }

        public void Delete<T>(T entity) where T : class
        {
            appDbContext.Remove(entity);
        }

    }
}
