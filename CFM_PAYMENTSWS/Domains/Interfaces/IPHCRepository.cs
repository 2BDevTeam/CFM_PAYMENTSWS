﻿using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interfaces
{
    public interface IPHCRepository<TContext> where TContext : DbContext
    {


        /*
        public Ft GetFt(string ftstamp);
        public Ft2 GetFt2(string ft2stamp);

        public Para1 GetPara1(string descricao);
        public List<Ml> GetMlByDate(DateTime initialDate, DateTime finalDate);
        public Para1 GetPara1ByDescricao(string descricao);
        public E1 GetE1ByEstab(int estab);
        */
    }
}