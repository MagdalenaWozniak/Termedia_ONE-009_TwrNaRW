using cdn_api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZnaCSV
{
    public class Twr
    {
        public int GIDNumer { get; set; }
        public int GIDTyp { get; set; }
        public int GIDFirma {get; set; }
        public short GIDLp { get; set; }

        public Twr(int GIDNumer, int GIDTyp, int GIDFirma, short GIDLp)
        {
            this.GIDNumer = GIDNumer;
            this.GIDTyp = GIDTyp;
            this.GIDFirma = GIDFirma;
            this.GIDLp = GIDLp;
        }
        


        public string TowarLubUsluga(string connection_string)
        {
            try
            {
                string wynikQuery = "";
                string typQuery = "SELECT [Twr_Typ] FROM [CDN].[TwrKarty] WHERE Twr_GIDTyp = " + GIDTyp+" AND Twr_GIDNumer = "+GIDNumer+" and Twr_GIDLp= "+GIDLp + " and Twr_GIDFirma = "+GIDFirma;
                wynikQuery = ConnectToSQL(connection_string, typQuery);
                return wynikQuery;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas sprawdzania typu towaru: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }
        }


       

        public bool czyJestUS(string connection_string, string twr_kod)
        {
            try
            {
                twr_kod = twr_kod.TrimEnd();
                twr_kod = twr_kod + " US ";
                string wynikQuery = "";
                string usQuery = "SELECT count(*) FROM [CDN].[TwrKarty] WHERE Twr_Kod = '"+twr_kod+"'";
                wynikQuery = ConnectToSQL(connection_string, usQuery);
                if (wynikQuery == "1")
                    return true;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas szukania usługi: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public bool czyIstniejeTwr(string connection_string, string twrKod)
        {
            try
            {
                //strin ilosc = 1;
                string wynikQuery = "";
                twrKod = twrKod.TrimEnd();
                twrKod = twrKod.Substring(0, twrKod.Length - 3);
                //string iloscQuery = "SELECT [TrE_Ilosc] FROM [CDN].[TraElem] WHERE TrE_GIDTyp = " + GIDTyp + " AND TrE_GIDNumer = " + GIDNumer + " and TrE_GIDLp= " + pozycja;
                //ilosc = ConnectToSQL(connection_string, iloscQuery);
                //źle to nie będzie ten sam towar tylko towar z kodem bez US, czyli jesteś w dupie xd
                string zasobQuery = "SELECT SUM([TwZ_Ilosc]) FROM [CDN].[TwrZasoby] join CDN.TwrKarty as k on TwZ_TwrFirma = k.Twr_GIDFirma and TwZ_TwrNumer = k.Twr_GIDNumer and TwZ_TwrTyp = k.Twr_GIDTyp WHERE k.Twr_Kod = '" + twrKod + "' ";
                wynikQuery = ConnectToSQL(connection_string, zasobQuery);
                //MessageBox.Show("Twr_Kod: " + twrKod+"\nQuery: "+ zasobQuery+"\nwynik: "+wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (wynikQuery != "")
                {
                    //ilosc = ilosc.Substring(0, ilosc.IndexOf(","));
                    wynikQuery = wynikQuery.Substring(0, wynikQuery.IndexOf(","));
                    //MessageBox.Show("Ilosc: " + ilosc + "\nZasob: "+wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Convert.ToInt32(wynikQuery) > 0)
                    {
                        //MessageBox.Show("Ilosc: " + wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    return false;
                }

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas szukania zasobu: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public string czyUslugaBezUS( string connection_string)
        {
            try
            {
                string wynikQuery = "";
                string kodQuery = "SELECT Twr_Kod FROM [CDN].[TwrKarty] WHERE Twr_GIDTyp = " + GIDTyp + " AND Twr_GIDNumer = " + GIDNumer + " and Twr_GIDLp= " + GIDLp + " and Twr_GIDFirma = " + GIDFirma; ;
                wynikQuery = ConnectToSQL(connection_string, kodQuery);
                //MessageBox.Show("TwrKod: " + wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return wynikQuery;
               
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd w funkcji czyUslugaBezUS: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }

        }

       

        public string czyCzasopisma(string connection_string, string twrKod)
        {
            try
            {
                string wynikQuery = "";
                string grupaQuery = "SELECT CDN.TwrGrupaPelnaNazwa(TwG_GrONumer) FROM [CDN].[TwrGrupy] WHERE TwG_Kod='" + twrKod + "'";
                wynikQuery = ConnectToSQL(connection_string, grupaQuery);
                //MessageBox.Show("TwrKod: " + wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return wynikQuery;

            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd w funkcji czyCzasopisma: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }

        }


        public string ConnectToSQL(string connection_string, string query)
        {
            string str = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "Query");
                    if (dataSet.Tables.Count == 0)
                    {
                        return "";
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        str = row[0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                str = str + "Bład SQL:" + ex.ToString() + "\n";
            }
            return str;
        }


    }
}
