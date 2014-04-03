using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;

using Retail.Model;
using RetailUsingEntityFramework.Mapping;

namespace RetailUsingEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            // buat objek item beli (3 objek)
            var item1 = new ItemBeli
            {
                BarangID = "BB-7421",
                Jumlah = 5,
                HargaBeli = 54,
                HargaJual = 60
            };

            var item2 = new ItemBeli
            {
                BarangID = "BB-8107",
                Jumlah = 7,
                HargaBeli = 101,
                HargaJual = 110
            };

            var item3 = new ItemBeli
            {
                BarangID = "BK-M18B-44",
                Jumlah = 10,
                HargaBeli = 540,
                HargaJual = 693
            };
            
            // buat objek beli
            var beli = new Beli
            {
                Nota = "N001",
                SupplierID = 3,
                Tanggal = DateTime.Today,
                Keterangan = "Transaksi beli menggunakan Entity Framework"
            };

            // tambahkan item beli ke objek beli
            beli.ItemBelis.Add(item1);
            beli.ItemBelis.Add(item2);
            beli.ItemBelis.Add(item3);

            var result = AddPembelianUsingEF(beli);

            Console.WriteLine("\nStatus transaksi : {0}", result == 1 ? "Sukses" : "Gagal");
            Console.ReadKey();
        }

        private static int AddPembelianUsingEF(Beli beli)
        {
            var result = 0;

            using (var db = new RetailContext())
            {
                try
                {
                    db.Entry(beli).State = EntityState.Added;
                    db.SaveChanges();

                    result = 1;
                }
                catch
                {
                }
            }

            return result;
        }
    }
}
