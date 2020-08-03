using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosTest.Models;
using Microsoft.AspNetCore.SignalR;

namespace KairosTest.Interface
{
   public interface IRentBook
    {
        int Create(RentBook data);
        List<RentBook> GetList();
    }
}
