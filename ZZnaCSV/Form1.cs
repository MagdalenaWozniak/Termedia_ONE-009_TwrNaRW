using cdn_api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZnaCSV
{
    public partial class Form1 : Form
    {
        string kodTwr = "";
        string connection_string = "";
        int sesja = 0;
        private int XLWersjaAPI = 20202;
        public Form1(string kod_Twr, string connectionString, int iSesja)
        {
            InitializeComponent();
            kodTwr = kod_Twr;
            connection_string = connectionString;
            sesja = iSesja;
            
        }

        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                //coś  jest nie tak
                string date3 = data2.Value.ToString("yyyy-MM-dd"); 
                //MessageBox.Show("Data3:" + date3, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                int dataFS = getDate(date3);

                //MessageBox.Show("DataFS:" + dataFS, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string FSyQuery = "SELECT [TrE_GIDTyp] ,[TrE_GIDFirma],[TrE_GIDNumer] ,[TrE_GIDLp] FROM [CDN].[TraElem] as e JOIN CDN.TraNag as n on e.TrE_GIDFirma = n.TrN_GIDFirma and e.TrE_GIDNumer = n.TrN_GIDNumer left join cdn.Atrybuty on Atr_ObiTyp= TrE_GIDTyp AND Atr_ObiNumer= TrE_GIDNumer and Atr_ObiLp= TrE_GIDLp and Atr_AtkId=12 " +
                    "WHERE TrE_TwrKod = '"+kodTwr+"'  and n.TrN_TrNSeria <> 'PRO' and TrN_TrNTyp in (3,10,20) and (Atr_Wartosc is null or Atr_Wartosc='') and TrN_Data2>= " + dataFS + "and TrN_NrKorekty=''";


                //MessageBox.Show("FSQuery:" + FSyQuery, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<FS> FSy = SelectListRowColumn(FSyQuery, "CDN.TraElem");
                foreach(FS faktura in FSy)
                {
                    //MessageBox.Show("Numer :" + faktura.GIDNumer);
                    dodawanieDokumentu(faktura);
                    
                }
                

                

            }
            catch(Exception en)
            {
                MessageBox.Show("Błąd: "+en,"Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show("Zakończono generowanie RW", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        public void dodawanieDokumentu(FS faktura)
        {

            int dokumentID = 0;
            try
            {
                // MessageBox.Show("Dodawanie faktury", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string traNagQuery = "SELECT * FROM CDN.TraNag WHERE TrN_GIDTyp=" + faktura.GIDTyp + " and TrN_GIDFirma=" + faktura.GIDFirma + " and TrN_GIDNumer=" + faktura.GIDNumer ;
                List<string> traNag = SelectListColumn(traNagQuery, "CDN.TraNag");
                // MessageBox.Show("KntTyp = "+(traNag[16] != null ? (Convert.ToInt32(traNag[16])) : 0)+"\nKntFirma = "+(traNag[17] != null ? (Convert.ToInt32(traNag[17])) : 0)+ "\nKntNumer = "+(traNag[18] != null ? (Convert.ToInt32(traNag[18])) : 0)+ "\nKntLp = "+(traNag[19] != null ? (Convert.ToInt32(traNag[19])) : 0), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string date4 = data1.Value.ToString("yyyy-MM-dd");
                int date = getDate(date4);
                int miesiac = data1.Value.Month;
                int rok = data1.Value.Year;

                //bool jestNaRW = faktura.czyJestNaRW(connection_string);
                //MessageBox.Show("Nazwa usługi: "+nazwaUslugi, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //if (!jestNaRW)
               // {

                    //MessageBox.Show("Zawiera US", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bool twr = faktura.czyJestTwr(connection_string, kodTwr);
                    if (twr)
                    {

                        #region dodawanie dokumentu
                        XLDokumentNagInfo_20202 nowyFS = new XLDokumentNagInfo_20202();
                        nowyFS.GIDFirma = 0;
                        nowyFS.GIDLp = 0;
                        nowyFS.GIDNumer = 0;
                        nowyFS.GIDTyp = 0;
                        nowyFS.Typ = 1616;
                        nowyFS.Tryb = 2;
                        nowyFS.Wersja = XLWersjaAPI;
                        nowyFS.Seria = "<PUS>";
                        nowyFS.ZwrTyp = (traNag[4] != null ? (Convert.ToInt32(traNag[4])) : 0);
                        nowyFS.ZwrFirma = (traNag[5] != null ? (Convert.ToInt32(traNag[5])) : 0);
                        nowyFS.ZwrNumer = (traNag[6] != null ? (Convert.ToInt32(traNag[6])) : 0);
                        nowyFS.ZwrLp = (traNag[7] != null ? (Convert.ToInt32(traNag[7])) : 0);
                        nowyFS.KntTyp = (traNag[16] != null ? (Convert.ToInt32(traNag[16])) : 0);
                        nowyFS.KntFirma = (traNag[17] != null ? (Convert.ToInt32(traNag[17])) : 0);
                        nowyFS.KntNumer = (traNag[18] != null ? (Convert.ToInt32(traNag[18])) : 0);
                        nowyFS.KntLp = (traNag[19] != null ? (Convert.ToInt32(traNag[19])) : 0);
                        nowyFS.AkwTyp = (traNag[24] != null ? (Convert.ToInt32(traNag[24])) : 0);
                        nowyFS.AkwFirma = (traNag[25] != null ? (Convert.ToInt32(traNag[25])) : 0);
                        nowyFS.AkwNumer = (traNag[26] != null ? (Convert.ToInt32(traNag[26])) : 0);
                        nowyFS.AkwLp = (traNag[27] != null ? (Convert.ToInt32(traNag[27])) : 0);
                        nowyFS.AdwTyp = (traNag[28] != null ? (Convert.ToInt32(traNag[28])) : 0);
                        nowyFS.AdwFirma = (traNag[29] != null ? (Convert.ToInt32(traNag[29])) : 0);
                        nowyFS.AdwNumer = (traNag[30] != null ? (Convert.ToInt32(traNag[30])) : 0);
                        nowyFS.AdwLp = (traNag[31] != null ? (Convert.ToInt32(traNag[31])) : 0);
                        nowyFS.ZamTyp = (traNag[36] != null ? (Convert.ToInt32(traNag[36])) : 0);
                        nowyFS.ZamFirma = (traNag[37] != null ? (Convert.ToInt32(traNag[37])) : 0);
                        nowyFS.ZamNumer = (traNag[38] != null ? (Convert.ToInt32(traNag[38])) : 0);
                        nowyFS.ZamLp = (traNag[39] != null ? (Convert.ToInt32(traNag[39])) : 0);
                        nowyFS.SaNTyp = (traNag[40] != null ? (Convert.ToInt32(traNag[40])) : 0);
                        nowyFS.SaNFirma = (traNag[41] != null ? (Convert.ToInt32(traNag[41])) : 0);
                        nowyFS.SaNNumer = (traNag[42] != null ? (Convert.ToInt32(traNag[42])) : 0);
                        nowyFS.SaNLp = (traNag[43] != null ? (Convert.ToInt32(traNag[43])) : 0);
                        nowyFS.KonTyp = (traNag[44] != null ? (Convert.ToInt32(traNag[44])) : 0);
                        nowyFS.KonNumer = (traNag[46] != null ? (Convert.ToInt32(traNag[46])) : 0);
                        nowyFS.Data = date;     //(traNag[53] != null ? (Convert.ToInt32(traNag[53])) : 0);
                        nowyFS.DataSpr = date;   //(traNag[54] != null ? (Convert.ToInt32(traNag[54])) : 0);
                        nowyFS.Rok = rok; // (traNag[56] != null ? (Convert.ToInt32(traNag[56])) : 0);
                        nowyFS.Miesiac = miesiac; // (traNag[57] != null ? (Convert.ToInt32(traNag[57])) : 0);
                        nowyFS.Rejestr = (traNag[62] != null ? (traNag[62]) : "");
                        nowyFS.RokVAT = (traNag[63] != null ? (Convert.ToInt32(traNag[63])) : 0);
                        nowyFS.MiesiacVAT = (traNag[64] != null ? (Convert.ToInt32(traNag[64])) : 0);
                        nowyFS.Rozliczac = (traNag[70] != null ? (Convert.ToInt32(traNag[70])) : 0);
                        //nowyFS.DokumentObcy = (traNag[71] != null ? (traNag[71]) : "");
                        nowyFS.OdliczeniaVat = (traNag[75] != null ? (Convert.ToInt32(traNag[75])) : 0);
                        nowyFS.RodzajZakupu = (traNag[76] != null ? (Convert.ToInt32(traNag[76])) : 0);
                        nowyFS.ExpoNorm = (traNag[77] != null ? (Convert.ToInt32(traNag[77])) : 0);
                        nowyFS.NB = (traNag[79] != null ? (traNag[79]) : "");
                        nowyFS.Fiskalny = (traNag[81] != null ? (Convert.ToInt32(traNag[81])) : 0);
                        nowyFS.Forma = (traNag[82] != null ? (Convert.ToInt32(traNag[82])) : 0);
                        nowyFS.Rabat = (traNag[85] != null ? Convert.ToInt32((Convert.ToDecimal(traNag[85]) * 100)) : 0);
                        nowyFS.SposobDst = (traNag[92] != null ? (traNag[92]) : "");
                        nowyFS.Waluta = (traNag[117] != null ? (traNag[117]) : "");
                        nowyFS.KursNr = (traNag[118] != null ? (Convert.ToInt32(traNag[118])) : 0);
                        nowyFS.KursM = (traNag[119] != null ? (Convert.ToInt32(traNag[119])) : 1);
                        nowyFS.KursL = (traNag[120] != null ? (Convert.ToInt32(Convert.ToDecimal(traNag[120])) * 100) : 1);
                        //nowyFS.FRSID = ((traNag[125] != null && traNag[125] != "") ? (Convert.ToInt32(traNag[125].Trim())) : 0);
                        nowyFS.Cecha = (traNag[126] != null ? (traNag[126]) : "");
                        nowyFS.KnDTyp = (traNag[134] != null ? (Convert.ToInt32(traNag[134])) : 0);
                        nowyFS.KnDFirma = (traNag[135] != null ? (Convert.ToInt32(traNag[135])) : 0);
                        nowyFS.KnDNumer = (traNag[136] != null ? (Convert.ToInt32(traNag[136])) : 0);
                        nowyFS.KnDLp = (traNag[137] != null ? (Convert.ToInt32(traNag[137])) : 0);
                        nowyFS.URL = (traNag[142] != null ? (traNag[142]) : "");
                        nowyFS.Termin = (traNag[144] != null ? (Convert.ToInt32(traNag[144])) : 0);
                        nowyFS.KodRodzajuTransportu = (traNag[150] != null ? (traNag[150]) : "");
                        nowyFS.KarNumer = (traNag[132] != null ? (Convert.ToInt32(traNag[132])) : 0);



                        int bladDok = cdn_api.cdn_api.XLNowyDokument(sesja, ref dokumentID, nowyFS);
                        if (bladDok != 0)
                            MessageBox.Show("Błąd podczas dodawania dokumentu API: " + bladDok, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #endregion

               

                    /*string typTwr = faktura.TowarLubUsluga( connection_string);
                    int iTypTwr = Convert.ToInt32(typTwr);
                    //MessageBox.Show("Typ towaru: "+typTwr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);*/

                    string traElemQuery = "SELECT * FROM CDN.TraElem WHERE TrE_GIDTyp=" + faktura.GIDTyp + " and TrE_GIDFirma=" + faktura.GIDFirma + " and TrE_GIDNumer=" + faktura.GIDNumer + " and TrE_GIDLp = " + faktura.GIDLp;
                    List<string> traElem = SelectListColumn(traElemQuery, "CDN.TraElem");
                    string twrKod = traElem[10].TrimEnd();
                    //int counter = 0;
                    
                   /* if (iTypTwr == 4)
                    {
                        
                        string nazwaUslugi = faktura.czyUslugaBezUS(pozycja, connectionString);*/
                       

                                twrKod = twrKod.Substring(0, twrKod.Length - 3);
                                //counter++;
                                #region dodawanie pozycji
                                //MessageBox.Show( "Wartosc: " + (traElem[63] != null ? traElem[63] : ""), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                                XLDokumentElemInfo_20202 nowyTowar = new XLDokumentElemInfo_20202();
                                nowyTowar.TowarKod = twrKod;
                                nowyTowar.Ilosc = (traElem[22] != null ? (traElem[22]) : "");
                                //nowyTowar.JmFormat = (traElem[23] != null ? Convert.ToInt32(traElem[23]) : 0);
                                nowyTowar.JmZ = (traElem[24] != null ? traElem[24] : "");
                                //nowyTowar.TypJm = (traElem[26] != null ? Convert.ToInt32(traElem[26]) : 0);
                                //nowyTowar.PrzeliczM = (traElem[27] != null ? Convert.ToInt32(traElem[27]) : 0);
                                //nowyTowar.PrzeliczL = (traElem[28] != null ? Convert.ToInt32(traElem[28]) : 0);
                                //nowyTowar.StawkaPod = (traElem[30] != null ? Convert.ToInt32(traElem[30]) : 0);
                                //nowyTowar.FlagaVAT = (traElem[31] != null ? Convert.ToInt32(traElem[31]) : 0);
                                nowyTowar.Waluta = (traElem[34] != null ? traElem[34] : "");
                                //nowyTowar.KursNr = (traElem[35] != null ? Convert.ToInt32(traElem[35]) : 0);
                                //nowyTowar.KursM = (traElem[36] != null ? Convert.ToInt32(traElem[36]) : 1);
                                //nowyTowar.KursL = (traElem[37] != null ? Convert.ToInt32(Convert.ToDecimal(traElem[37])*100) : 1);
                                //nowyTowar.Rabat = (traElem[40] != null ? Convert.ToInt32(Convert.ToDecimal(traElem[40])*100) : 0);
                                //nowyTowar.RecTyp = (traElem[59] != null ? Convert.ToInt32(traElem[59]) : 0);
                                //nowyTowar.RecFirma = (traElem[60] != null ? Convert.ToInt32(traElem[60]) : 0);
                                //nowyTowar.RecNumer = (traElem[61] != null ? Convert.ToInt32(traElem[61]) : 0);
                                //nowyTowar.RecLp = (traElem[62] != null ? Convert.ToInt32(traElem[62]) : 0);
                                nowyTowar.Cena = (traElem[63] != null ? traElem[63] : "");
                                /* nowyTowar.Rownanie = (traElem[64] != null ? Convert.ToInt32(traElem[64]) : 0);
                                 nowyTowar.OdKsiegowych = (traElem[65] != null ? Convert.ToInt32(traElem[65]) : 0);
                                 nowyTowar.PCN = (traElem[67] != null ? traElem[67] : "");
                                 nowyTowar.PrecyzjaCeny = (traElem[68] != null ? Convert.ToInt32(traElem[68]) : 0);
                                 nowyTowar.Kraj = (traElem[76] != null ? (traElem[76]) : "");
                                 nowyTowar.CenaSpr = (traElem[89] != null ? Convert.ToInt32(traElem[89]) : 0);
                                 nowyTowar.CChTyp = (traElem[91] != null ? Convert.ToInt32(traElem[91]) : 0);
                                 nowyTowar.CChNumer = (traElem[92] != null ? Convert.ToInt32(traElem[92]) : 0);
                                 nowyTowar.Cecha = (traElem[93] != null ? traElem[93] : "");
                                 nowyTowar.PakietId = (traElem[97] != null ? Convert.ToInt32(traElem[97]) : 0);
                                 nowyTowar.Gratis = (traElem[98] != null ? Convert.ToInt32(traElem[98]) : 0);
                                 nowyTowar.PromocjaProgID = (traElem[99] != null ? Convert.ToInt32(traElem[99]) : 0);
                                 nowyTowar.Zlom = (traElem[101] != null ? Convert.ToInt32(traElem[101]) : 0);*/
                     nowyTowar.Wersja = XLWersjaAPI;

                      int blad_towar = cdn_api.cdn_api.XLDodajPozycje(dokumentID, nowyTowar);
                      if (blad_towar != 0)
                          MessageBox.Show("Błąd podczas dodawania pozycji API: " + blad_towar, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    #endregion

                    //MessageBox.Show("NowyTowar.Cena: "+nowyTowar.Cena, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (bladDok == 0 && blad_towar == 0)
                    {
                        #region dodawanie atrybutów
                        XLAtrybutInfo_20202 atr1 = new XLAtrybutInfo_20202();
                        atr1.GIDFirma = nowyFS.GIDFirma;
                        atr1.GIDLp = 1;
                        atr1.GIDNumer = nowyFS.GIDNumer;
                        atr1.GIDTyp = nowyFS.GIDTyp;
                        atr1.GIDSubLp = 0;
                        atr1.Klasa = "czasopisma_FS";
                        atr1.Wartosc = (traNag[71] != null ? (traNag[71]) : "");
                        atr1.Wersja = XLWersjaAPI;

                        int bladAtr = cdn_api.cdn_api.XLDodajAtrybut(sesja, atr1);
                        if (bladAtr != 0)
                            MessageBox.Show("Błąd podczas dodawania atrybutu czasopisma_FS: " + bladAtr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                        string nrFaktury = faktura.numerFs(nowyFS, connection_string);

                        //MessageBox.Show("Ilosc: "+ (traElem[22] != null ? (traElem[22]) : "").Replace(",", ".") +"\nWartosc: "+ (traElem[63] != null ? traElem[63] : "").Replace(",", "."), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        string ilosc = (traElem[22] != null ? (traElem[22]) : "").Trim();
                        string wartosc = (traElem[63] != null ? traElem[63] : "").Trim();
                        NumberFormatInfo formatProvider = new NumberFormatInfo();
                        formatProvider.NumberDecimalSeparator = ",";
                        formatProvider.NumberGroupSeparator = ".";
                        formatProvider.NumberGroupSizes = new int[] { 2 };
                        XLAtrybutInfo_20202 atr2 = new XLAtrybutInfo_20202();
                        atr2.GIDFirma = nowyFS.GIDFirma;
                        atr2.GIDLp = 1;
                        atr2.GIDNumer = nowyFS.GIDNumer;
                        atr2.GIDTyp = nowyFS.GIDTyp;
                        atr2.GIDSubLp = 0;
                        atr2.Klasa = "Wartosc Sprzedazy Prenumeraty";
                        atr2.Wartosc = String.Format("{0:N2}", (Convert.ToDouble((traElem[22] != null ? (traElem[22]) : ""), formatProvider) * Convert.ToDouble((traElem[63] != null ? traElem[63] : ""), formatProvider))).Replace(".", ",");
                        atr2.Wersja = XLWersjaAPI;

                        bladAtr = cdn_api.cdn_api.XLDodajAtrybut(sesja, atr2);
                        if (bladAtr != 0)
                            MessageBox.Show("Błąd podczas dodawania atrybutu czasopisma_wartosc: " + bladAtr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        XLAtrybutInfo_20202 atr4 = new XLAtrybutInfo_20202();
                        atr4.GIDFirma = faktura.GIDFirma;
                        atr4.GIDLp = faktura.GIDLp;
                        atr4.GIDNumer = faktura.GIDNumer;
                        atr4.GIDTyp = faktura.GIDTyp;
                        atr4.GIDSubLp = 0;
                        atr4.Klasa = "czasopisma_wydano";
                        atr4.Wartosc = String.Format("{0:N2}", (Convert.ToDouble((traElem[22] != null ? (traElem[22]) : ""), formatProvider))).Replace(".", ",");
                        atr4.Wersja = XLWersjaAPI;

                        bladAtr = cdn_api.cdn_api.XLDodajAtrybut(sesja, atr4);
                        if (bladAtr != 0)
                            MessageBox.Show("Błąd podczas dodawania atrybutu czasopisma_wydano: " + bladAtr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                        XLAtrybutInfo_20202 atr3 = new XLAtrybutInfo_20202();
                        atr3.GIDFirma = faktura.GIDFirma;
                        atr3.GIDLp = faktura.GIDLp;
                        atr3.GIDNumer = faktura.GIDNumer;
                        atr3.GIDTyp = faktura.GIDTyp;
                        atr3.GIDSubLp = 0;
                        atr3.Klasa = "czasopisma_dokument";
                        atr3.Wartosc = nrFaktury;
                        atr3.Wersja = XLWersjaAPI;

                        bladAtr = cdn_api.cdn_api.XLDodajAtrybut(sesja, atr3);
                        if (bladAtr != 0)
                            MessageBox.Show("Błąd podczas dodawania atrybutu czasopisma_dokument: " + bladAtr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        XLAtrybutInfo_20202 atr5 = new XLAtrybutInfo_20202();
                        atr5.GIDFirma = nowyFS.GIDFirma;
                        atr5.GIDLp = 0;
                        atr5.GIDNumer = nowyFS.GIDNumer;
                        atr5.GIDTyp = nowyFS.GIDTyp;
                        atr5.GIDSubLp = 0;
                        atr5.Klasa = "czasopisma_FS";
                        atr5.Wartosc = (traNag[71] != null ? (traNag[71]) : "");
                        atr5.Wersja = XLWersjaAPI;

                        bladAtr = cdn_api.cdn_api.XLDodajAtrybut(sesja, atr5);
                        if (bladAtr != 0)
                            MessageBox.Show("Błąd podczas dodawania atrybutu czasopisma_FS na RW: " + bladAtr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                        #endregion
                    }


                    traElem.Clear();

                  }






            //  }
         // }
             //   }


                #region zamykanie dokumentu
                XLZamkniecieDokumentuInfo_20202 closeDokument = new XLZamkniecieDokumentuInfo_20202();
                closeDokument.Wersja = XLWersjaAPI;
                closeDokument.Tryb = 1;
                cdn_api.cdn_api.XLZamknijDokument(dokumentID, closeDokument);
                traNag.Clear();

                #endregion


            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd dodawania faktury: " + e, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XLZamkniecieDokumentuInfo_20202 closeDokument = new XLZamkniecieDokumentuInfo_20202();
                closeDokument.Wersja = XLWersjaAPI;
                closeDokument.Tryb = 1;
                cdn_api.cdn_api.XLZamknijDokument(dokumentID, closeDokument);

            }

        }


        public int getDate(string date3)
        {
            string date1 = "1800-12-28";
            //string date3 = DateTime.Now.ToString("yyyy-MM-dd");
            string Datequery = "SELECT DATEDIFF(day, '" + date1 + "','" + date3 + "'); ";
            return Convert.ToInt32(ConnectToSQL( Datequery));
        }

        public List<string> SelectListColumn(string query, string srcTableName)
        {
            List<string> str = new List<string>();
            string sttr = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, srcTableName);
                    foreach (DataColumn column in (InternalDataCollectionBase)dataSet.Tables[0].Columns)
                        foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                            str.Add(row[column].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
            }
            return str;
        }

        public string ConnectToSQL(string query)
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


        public List<FS> SelectListRowColumn(string query, string srcTableName)
        {
            List<string> str = new List<string>();
            List<FS> myList = new List<FS>();
            string sttr = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, srcTableName);
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        foreach (DataColumn column in (InternalDataCollectionBase)dataSet.Tables[0].Columns)
                        {
                            str.Add(row[column].ToString());

                            //MessageBox.Show(column + ": " + row[column].ToString());
                        }
                        myList.Add(new FS(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), (short)Convert.ToInt32(str[3])));
                        str.Clear();

                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
            }
            return myList;
        }
    }
}
