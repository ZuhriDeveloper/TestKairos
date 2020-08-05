using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosTest.Models;

namespace KairosTest.Interface
{
   public interface IBook
    {
        Book GetBookByID(int id);
        List<Book> GetListBook();

        int Create(Book data);
        void Update(Book data);

        void Delete(int id);
    }
}
