using System;
using ClickHouseTableGenerator;
using DbContext;

namespace UnitTests.TestDbContext
{
    // Int8 - int
    // Int16-int
    // Int32 - int
    //Int64 - long

    public class YandexMetrikaDbSetSettingProvider : ICreatingDbSettingsProvider
    {
        public EngineDbInfo Create()
        {
            return new EngineDbInfo
            {
                OrderKeys = new[] {"CounterID", " EventDate", "intHash32(UserID)"},
                PartitionKeys = new[] {"toYYYYMM(EventDate)"},
                Samples = new[] {"intHash32(UserID)"}
            };
        }
    }

    [TableName("hits_v1")]
    [DbSetSettings(typeof(YandexMetrikaDbSetSettingProvider))]
    public class YandexMetrikaTestTable
    {
        public ulong WatchID { get; set; }
        public int JavaEnable { get; set; }
        public string Title { get; set; }
        public int GoodEvent { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime EventDate { get; set; }
        public long CounterID { get; set; }
        public long ClientIP { get; set; }
        public long RegionID { get; set; }
        public ulong UserID { get; set; }
        public int CounterClass { get; set; }
        public int OS { get; set; }
        public int UserAgent { get; set; }
        public string URL { get; set; }
        public string Referer { get; set; }
        public string URLDomain { get; set; }
        public string RefererDomain { get; set; }
        public int Refresh { get; set; }

        public int IsRobot { get; set; }

//        public int[] RefererCategories { get; set; }
//        public long[] URLCategories { get; set; }
//        public long[] URLRegions { get; set; }
        public long[] RefererRegions { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public int ResolutionDepth { get; set; }
        public int FlashMajor { get; set; }
        public int FlashMinor { get; set; }
        public string FlashMinor2 { get; set; }
        public int NetMajor { get; set; }
        public int NetMinor { get; set; }
        public long UserAgentMajor { get; set; }
        public string UserAgentMinor { get; set; }
        public int CookieEnable { get; set; }

        public int JavascriptEnable { get; set; }

//
//        //bool
        public int IsMobile { get; set; }

//
//        //
        public int MobilePhone { get; set; }
        public string Params { get; set; }
        public long IPNetworkID { get; set; }
        public int TraficSourceID { get; set; }
        public int SearchEngineID { get; set; }

        public string SearchPhrase { get; set; }

//
        public int AdvEngineID { get; set; }

//
//        //bool
        public int IsArtifical { get; set; }

//
//        //
        public int WindowClientWidth { get; set; }
        public int WindowClientHeight { get; set; }
        public int ClientTimeZone { get; set; }
        public DateTime ClientEventTime { get; set; }
        public int SilverlightVersion1 { get; set; }
        public int SilverlightVersion2 { get; set; }
        public long SilverlightVersion3 { get; set; }
        public long SilverlightVersion4 { get; set; }
        public string PageCharset { get; set; }

        public long CodeVersion { get; set; }

//        //bool
        public int IsLink { get; set; }
        public int IsDownload { get; set; }

        public int IsNotBounce { get; set; }

//        //
        public ulong FUniqID { get; set; }

        public long HID { get; set; }

//        //bool
        public int IsOldCounter { get; set; }
        public int IsEvent { get; set; }

        public int IsParameter { get; set; }

//        //
        public int DontCountHits { get; set; }
        public int WithHash { get; set; }
        public string HitColor { get; set; }
        public DateTime UTCEventTime { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public int Income { get; set; }

        public int Interests { get; set; }

//
        public int Robotness { get; set; }

//
//        public int[] GeneralInterests { get; set; }
        public long RemoteIP { get; set; }
        public string RemoteIP6 { get; set; }
        public int WindowName { get; set; }
        public int OpenerName { get; set; }
        public int HistoryLength { get; set; }
        public string BrowserLanguage { get; set; }
        public string BrowserCountry { get; set; }
        public string SocialNetwork { get; set; }
        public string SocialAction { get; set; }
        public int HTTPError { get; set; }
        public int SendTiming { get; set; }
        public int DNSTiming { get; set; }
        public int ConnectTiming { get; set; }
        public int ResponseStartTiming { get; set; }
        public int ResponseEndTiming { get; set; }
        public int FetchTiming { get; set; }
        public int RedirectTiming { get; set; }
        public int DOMInteractiveTiming { get; set; }
        public int DOMContentLoadedTiming { get; set; }
        public int DOMCompleteTiming { get; set; }
        public int LoadEventStartTiming { get; set; }
        public int LoadEventEndTiming { get; set; }
        public int NSToDOMContentLoadedTiming { get; set; }
        public int FirstPaintTiming { get; set; }
        public int RedirectCount { get; set; }
        public int SocialSourceNetworkID { get; set; }
        public string SocialSourcePage { get; set; }
        public long ParamPrice { get; set; }
        public string ParamOrderID { get; set; }

        public string ParamCurrency { get; set; }

//
//        public int ParamCurrencyID { get; set; }
//
//        public long[] GoalsReached { get; set; }
        public string OpenstatServiceName { get; set; }
        public string OpenstatCampaignID { get; set; }
        public string OpenstatAdID { get; set; }
        public string OpenstatSourceID { get; set; }
        public string UTMSource { get; set; }
        public string UTMMedium { get; set; }
        public string UTMCampaign { get; set; }
        public string UTMContent { get; set; }
        public string UTMTerm { get; set; }
        public string FromTag { get; set; }
        public int HasGCLID { get; set; }
        public ulong RefererHash { get; set; }
        public ulong URLHash { get; set; }
        public long CLID { get; set; }
        public ulong YCLID { get; set; }
        public string ShareService { get; set; }
        public string ShareURL { get; set; }
        public string ShareTitle { get; set; }
        public string IslandID { get; set; }
        public long RequestNum { get; set; }
        public int RequestTry { get; set; }
    }
}