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
    public class FS
    {
        public int GIDNumer { get; set; }
        public int GIDTyp { get; set; }
        public int GIDFirma { get; set; }
        public short GIDLp { get; set; }

        public FS(int GIDNumer, int GIDTyp, int GIDFirma, short GIDLp)
        {
            this.GIDNumer = GIDNumer;
            this.GIDTyp = GIDTyp;
            this.GIDFirma = GIDFirma;
            this.GIDLp = GIDLp;
        }



        


        public bool czyJestTwr( string connection_string, string twrKod)
        {
            try
            {
                string ilosc = "";
                string wynikQuery = "";
                twrKod = twrKod.TrimEnd();
                twrKod = twrKod.Substring(0, twrKod.Length - 3);
                string iloscQuery = "SELECT [TrE_Ilosc] FROM [CDN].[TraElem] WHERE TrE_GIDTyp = " + GIDTyp + " AND TrE_GIDNumer = " + GIDNumer + " and TrE_GIDLp= " + GIDLp;
                ilosc = ConnectToSQL(connection_string, iloscQuery);
                //źle to nie będzie ten sam towar tylko towar z kodem bez US, czyli jesteś w dupie xd
                string zasobQuery = "SELECT SUM([TwZ_Ilosc]) FROM [CDN].[TwrZasoby] join CDN.TwrKarty as k on TwZ_TwrFirma = k.Twr_GIDFirma and TwZ_TwrNumer = k.Twr_GIDNumer and TwZ_TwrTyp = k.Twr_GIDTyp WHERE k.Twr_Kod = '" + twrKod + "' ";
                wynikQuery = ConnectToSQL(connection_string, zasobQuery);
                //MessageBox.Show("Twr_Kod: " + twrKod+"\nQuery: "+ zasobQuery+"\nwynik: "+wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (wynikQuery != "")
                {
                    ilosc = ilosc.Substring(0, ilosc.IndexOf(","));
                    wynikQuery = wynikQuery.Substring(0, wynikQuery.IndexOf(","));
                    //MessageBox.Show("Ilosc: " + ilosc + "\nZasob: "+wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Convert.ToInt32(wynikQuery) >= Convert.ToInt32(ilosc))
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


        public string numerFs(XLDokumentNagInfo_20202 nowyFS, string connection_string)
        {
            string wynikQuery = "";
            string nrQuery = "SELECT CDN.NumerDokumentu(TrN_GIDTyp, TrN_SpiTyp, TrN_TrNTyp, TrN_TrNNumer,TrN_TrNRok, TrN_TrNSeria, TrN_TrNMiesiac) FROM CDN.TraNag WHERE TrN_GIDTyp =" + nowyFS.GIDTyp + " AND TrN_GIDNumer = " + nowyFS.GIDNumer + " and TrN_GIDLp = " + nowyFS.GIDLp + " and TrN_GIDFirma = " + nowyFS.GIDFirma;
            wynikQuery = ConnectToSQL(connection_string, nrQuery);
            return wynikQuery;
        }


        public bool czyJestNaRW( string connection_string)
        {
            try
            {
                string wynikQuery = "";
                string fsQuery = "SELECT Atr_Wartosc FROM [CDN].[Atrybuty] WHERE (Atr_ObiTyp=" + GIDTyp + " AND Atr_ObiNumer=" + GIDNumer + " and Atr_ObiLp= " + GIDLp + " and Atr_AtkId=12)";
                wynikQuery = ConnectToSQL(connection_string, fsQuery);
                //MessageBox.Show("Wartosc atrybutu na pozcyji: " + wynikQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (wynikQuery == null || wynikQuery == "")
                {
                    //MessageBox.Show("Return false, czyli git " , "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd podczas sprawdzania czy istnieje faktura: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
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
