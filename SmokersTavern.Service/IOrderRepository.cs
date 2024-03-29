﻿//Vishalen
using SmokersTavern.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokersTavern.Service
{
    public interface IOrderRepository : IDisposable
    {
        Order GetById(Int32 id);
        List<Order> GetAll();
        void Insert(Order model);
        void Update(Order model);
        void Delete(Order model);
        IEnumerable<Order> Find(Func<Order, bool> predicate);
    }
}
