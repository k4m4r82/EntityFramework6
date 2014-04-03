using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.SqlClient;
using Retail.Model;

namespace RetailUsingADONET
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
                Keterangan = "Transaksi beli menggunakan ADO.NET"
            };

            // tambahkan item beli ke objek beli
            beli.ItemBelis.Add(item1);
            beli.ItemBelis.Add(item2);
            beli.ItemBelis.Add(item3);

            var result = AddPembelianUsingADONET(beli);

            Console.WriteLine("\nStatus transaksi : {0}", result == 1 ? "Sukses" : "Gagal");
            Console.ReadKey();
        }

        private static int AddPembelianUsingADONET(Beli beli)
        {
            var result = 0;

            using (var conn = GetOpenConnection())
            {

                try
                {
                    // mulai transaksi
                    var transaction = conn.BeginTransaction();

                    var sql = @"INSERT INTO Beli (Nota, SupplierID, Tanggal, Keterangan) 
                                VALUES (@1, @2, @3, @4)";

                    // insert ke tabel beli
                    using (var cmd = new SqlCommand(sql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@1", beli.Nota);
                        cmd.Parameters.AddWithValue("@2", beli.SupplierID);
                        cmd.Parameters.AddWithValue("@3", beli.Tanggal);
                        cmd.Parameters.AddWithValue("@4", beli.Keterangan);

                        result = cmd.ExecuteNonQuery();
                    }

                    sql = @"INSERT INTO ItemBeli (Nota, BarangID, Jumlah, HargaBeli, HargaJual) 
                            VALUES (@1, @2, @3, @4, @5)";

                    // insert ke tabel item beli sebanyak n item
                    foreach (var item in beli.ItemBelis)
                    {
                        using (var cmd = new SqlCommand(sql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@1", beli.Nota);
                            cmd.Parameters.AddWithValue("@2", item.BarangID);
                            cmd.Parameters.AddWithValue("@3", item.Jumlah);
                            cmd.Parameters.AddWithValue("@4", item.HargaBeli);
                            cmd.Parameters.AddWithValue("@5", item.HargaJual);

                            result = cmd.ExecuteNonQuery();
                        }
                    }                    

                    // transaksi selesai
                    // simpan perubahan secara permanen
                    transaction.Commit();

                    result = 1; // status transaksi berhasil
                }
                catch
                {
                    result = 0;
                }
            }

            return result;
        }

        private static SqlConnection GetOpenConnection()
        {
            SqlConnection conn = null;

            try
            {
                var strConn = @"Data Source=.\sqlexpress2008;Initial Catalog=Retail;Integrated Security=True";

                conn = new SqlConnection(strConn);                
                conn.Open();

            }
            catch (Exception)
            {
            }

            return conn;
        }
    }
}
