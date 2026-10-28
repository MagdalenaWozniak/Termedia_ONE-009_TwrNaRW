using Hydra;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using ZZnaCSV;
using System.Collections.Generic;
using static ZZnaCSV.Twr;
using System.Text;
using System;
using System.Data.SqlClient;
using System.Data;
using cdn_api;
using System.Globalization;

[assembly: CallbackAssemblyDescription(Name: "ONE-009 Twr na RW",
                                           Description: "Generowanie RW dla zaznaczonego towaru",
                                           Author: "OneSupport",
                                           Version: "1.0",
                                           SystemVersion: "2021.0.2",
                                           Created: "01-04-2021")]


namespace ZZnaCSV
{
    [SubscribeProcedure(Procedures._TwrListaGrup, "Twr na RW")]
    public class ZSnaFS_Class : Callback
    {
        private int XLWersjaAPI = 20202;
        private int XLTrybWsadowy = 1;
        private int XLWinieta = -1;
        private int XLUtworzWlasnaSesje = 1;
        private int sesja = -1;
        private string XLSQLServer = "192.168.4.24";
        private string XLSQLDatabase = "ERPXL_Termedia_Test";//"ERPXL_PoloMarket"
        private string XLUser = "ADMIN"; //
        private string XLPass = "NE!w0rk12";//"Dupa2003!";
        private string connectionString = @"Data Source=192.168.4.24;Initial Catalog=ERPXL_Termedia_Test; User ID=onesupport;Password=a956!2A@";
        private ClaWindow RapButton;
        private ClaWindow zakladkaTabWewnetrzne;
        private ClaWindow RapButton2;
        private ClaWindow zakladkaTabWewnetrzne2;
        private ClaWindow filtrBtn;
        //private List<ListaZS> ZSy = new List<ListaZS>();
        private int listID;
        private bool bSpacja = false;
        long iNumer;
        long iTyp;
        long iFirma;
        long iLp;
        private ClaWindow listaPoz;
        public override void Init()
        {

            AddSubscription(true, 0, Events.JustAfterWindowOpening, new TakeEventDelegate(OnOpenWindow));
            AddSubscription(false, 0, Events.ResizeWindow, new TakeEventDelegate(Resize));
        }



        private bool OnOpenWindow(Procedures ProcId, int ControlId, Events Event)
        {

            ClaWindow Parent = GetWindow();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Rectangle WndBnd = GetWindow().Bounds;
            zakladkaTabWewnetrzne = GetWindow().AllChildren["?TabGrupyNazwy"];
            zakladkaTabWewnetrzne2 = GetWindow().AllChildren["?TabGrupyKody"];
            listaPoz = GetWindow().AllChildren["?Browse:1"];
            RapButton = zakladkaTabWewnetrzne.Children.Add(ControlTypes.button);
            RapButton2 = zakladkaTabWewnetrzne2.Children.Add(ControlTypes.button);
            RapButton.Visible = true;
            RapButton.TextRaw = "WYD";
            RapButton.ToolTipRaw = "Wydaj towary";
            //RapButton.IconRaw = "C:\\Onesupport\\FSnaRW\\wydanie2.ico";
            RapButton2.Visible = true;
            RapButton2.TextRaw = "WYD";
            RapButton2.ToolTipRaw = "Wydaj towary";
            //RapButton2.IconRaw = "C:\\Onesupport\\FSnaRW\\wydanie2.ico";

            filtrBtn = GetWindow().AllChildren["?BUTTONSeryjnyImpZal"];                            //pobiera współrzędne przycisku zapisz
            RapButton.Bounds = new Rectangle(filtrBtn.Bounds.X - 25, filtrBtn.Bounds.Y, 23, 15);
            RapButton2.Bounds = new Rectangle(filtrBtn.Bounds.X - 25, filtrBtn.Bounds.Y, 23, 15);
            //tu sprawdzic

            RapButton.OnAfterMouseDown += new TakeEventDelegate(BtnOnMouseDown);
            RapButton.OnBeforeAccepted += new TakeEventDelegate(zaczytaj_OnAfterAccepted);

            RapButton2.OnAfterMouseDown += new TakeEventDelegate(BtnOnMouseDown);
            RapButton2.OnBeforeAccepted += new TakeEventDelegate(zaczytaj_OnAfterAccepted);


            //pobrane z pola w zakładce Raporty
            return true;
        }

        public bool Resize(Procedures ProcId, int ControlId, Events Event)
        {
            filtrBtn = GetWindow().AllChildren["?BUTTONSeryjnyImpZal"];                            //pobiera współrzędne przycisku zapisz
            RapButton.Bounds = new Rectangle(filtrBtn.Bounds.X - 25, filtrBtn.Bounds.Y, 23, 15);
            //pobiera współrzędne przycisku zapisz
            RapButton2.Bounds = new Rectangle(filtrBtn.Bounds.X - 25, filtrBtn.Bounds.Y, 23, 15);
            return true;
        }

        bool zaczytaj_OnAfterAccepted(Procedures ProcedureId, int ControlId, Events Event)
        {
            bSpacja = true;
            Runtime.WindowController.PostEvent(listaPoz.Id, Events.NewSelection);
            iNumer = TwrKarty.Twr_GIDNumer;
            iFirma = TwrKarty.Twr_GIDFirma;
            iLp = TwrKarty.Twr_GIDLp;
            iTyp = TwrKarty.Twr_GIDTyp;
            

            return true;
        }

        public bool BtnOnMouseDown(Procedures ProcId, int ControlId, Events Event)
        {
            int dokumentID = 0;
            try
            {

                //ADODB._Recordset recordset = Runtime.WindowController.GetQueueMarked((int)ProcId, listID, GetCallbackThread());

               // MessageBox.Show(Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 18));
                if (Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 19) == "ERPXL_Termedia_TEST")
                {
                    //MessageBox.Show("Jestem tutaj", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    XLSQLDatabase = "Termedia TEST";
                    connectionString = @"Data Source=192.168.4.24;Initial Catalog=ERPXL_Termedia_Test; User ID=onesupport;Password=a956!2A@";
                }
                else if (Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 14) == "ERPXL_Termedia")
                {
                    XLSQLDatabase = "Termedia";
                    connectionString = @"Data Source=192.168.4.24;Initial Catalog=ERPXL_Termedia; User ID=onesupport;Password=a956!2A@";
                }
                else
                {
                    MessageBox.Show("Dodatek nie posiada funkcjonalności dla tej bazy.", "Komunikat",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;

                }


                #region logowanie
                XLLoginInfo_20202 nowyUzytkownik = new XLLoginInfo_20202();
                nowyUzytkownik.Wersja = XLWersjaAPI;
                nowyUzytkownik.Winieta = XLWinieta;
                nowyUzytkownik.TrybWsadowy = XLTrybWsadowy;
                nowyUzytkownik.UtworzWlasnaSesje = XLUtworzWlasnaSesje;
                nowyUzytkownik.SerwerKlucza = XLSQLServer;
                nowyUzytkownik.Baza = XLSQLDatabase;
                nowyUzytkownik.OpeIdent = XLUser;
                nowyUzytkownik.OpeHaslo = XLPass;
                nowyUzytkownik.PortHTTPSerweraKlucza = 1;
                nowyUzytkownik.RodzajSerweraKlucza = 0;
                nowyUzytkownik.SerwerKlucza = "localhost::5000185870";
                nowyUzytkownik.ProgramID = "ONE-009 Twr na RW";

                int blad = cdn_api.cdn_api.XLLogin(nowyUzytkownik, ref sesja);

                if (blad != 0)
                {
                    MessageBox.Show("Błąd logowania: "+blad, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                #endregion

                Twr towar = new Twr(Convert.ToInt32(iNumer), Convert.ToInt32(iTyp), Convert.ToInt32(iFirma), (short)iLp);


                string typTwr = towar.TowarLubUsluga(connectionString);
                int iTypTwr = Convert.ToInt32(typTwr);
                //MessageBox.Show("Typ towaru: "+typTwr, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bool istniejeTowar=false;
                bool twr = true;


                string nazwaUslugi = towar.czyUslugaBezUS(connectionString);

                string grupa = towar.czyCzasopisma(connectionString, nazwaUslugi);
                if (grupa.Contains("CZASOPISMA"))
                {
                    if (iTypTwr == 4)
                    {

                        //MessageBox.Show("Nazwa usługi: " + nazwaUslugi, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (nazwaUslugi.Contains(" US"))
                        {

                            //MessageBox.Show("Zawiera US", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            twr = towar.czyIstniejeTwr(connectionString, nazwaUslugi);
                            if (twr)
                            {
                                istniejeTowar = true;
                                //MessageBox.Show("Istnieje pozycja", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                    }
                    else if (iTypTwr == 1)
                    {
                        bool US = towar.czyJestUS(connectionString, nazwaUslugi);

                        if (US)
                        {
                            //coś mi tu nie działa
                            nazwaUslugi = nazwaUslugi.TrimEnd();
                            //MessageBox.Show("nazwaUslugi.TrimEnd(): "+nazwaUslugi, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            nazwaUslugi = nazwaUslugi + " US";
                            //MessageBox.Show("nazwaUslugi + US: " + nazwaUslugi, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            twr = towar.czyIstniejeTwr(connectionString, nazwaUslugi);
                            if (twr)
                                istniejeTowar = true;
                        }
                    }

                    if (istniejeTowar)
                    {
                        Application.Run(new Form1(nazwaUslugi, connectionString, sesja));
                    }
                    else
                    {
                        if(!twr)
                            MessageBox.Show("Brak zapasu na stanie", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            MessageBox.Show("Dla wybranej pozycji nie znaleziono towaru", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Wybrany towar nie należy do grupy \"CZASOPISMA\" ", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

               

                cdn_api.cdn_api.XLLogout(sesja);

            }
            catch {
                
            }
           
           
            return true;
        }

       
        public override void Cleanup()
        {

        }

       
        public List<string> SelectListColumn(string query, string srcTableName, string connectionString)
        {
            List<string> str = new List<string>();
            string sttr = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, srcTableName);
                    foreach (DataColumn column in (InternalDataCollectionBase)dataSet.Tables[0].Columns)
                        foreach(DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                        str.Add(row[column].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
            }
            return str;
        }

    }
}
