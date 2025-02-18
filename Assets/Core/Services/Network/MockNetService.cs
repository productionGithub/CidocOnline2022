﻿#define TRACE_OFF
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.IO;

namespace StarterCore.Core.Services.Network
{
    public class APIService
    {
        [Inject] private NetworkService _net;

        const string HomeUrl = "https://ontomatchgame.huma-num.fr/";
        const string LanguagesFolder = "StreamingAssets/Languages/";

        //GET
        private string URL_CHECK_USERNAME = Path.Combine(HomeUrl, "php/checkusername.php?username={0}");
        private string URL_GET_USERNAME = Path.Combine(HomeUrl, "php/getusername.php?email={0}");
        private string URL_CHECK_EMAIL = Path.Combine(HomeUrl, "php/checkemail.php?email={0}");
        private string URL_CHECK_STATUS = Path.Combine(HomeUrl, "php/checkstatus.php?email={0}");
        private string URL_LOGIN = Path.Combine(HomeUrl, "php/login.php");
        private string URL_GET_ACTIVATION_CODE = Path.Combine(HomeUrl, "php/getactivationcode.php");
        private string URL_SEND_RESET_EMAIL = Path.Combine(HomeUrl, "php/sendresetlink.php");
        private string URL_GET_HISTORY = Path.Combine(HomeUrl, "php/gethistory.php?userId={0}");
        private string URL_GET_USERID = Path.Combine(HomeUrl, "php/getuserid.php?email={0}");
        private string URL_GET_SESSION = Path.Combine(HomeUrl, "php/getsession.php?userId={0}?scenarioName={1}?chapterName={2}");
        private string URL_GET_USER_STATS = Path.Combine(HomeUrl, "php/getuserstats.php?userId={0}");
        private string URL_GET_PROGRESSION = Path.Combine(HomeUrl, "php/getprogression.php?UserId={0}&ScenarioName={1}&ChapterName={2}");
        private string URL_GET_RANKINGS = Path.Combine(HomeUrl, "php/getrankings.php");

        private string URL_GET_LOCALES_MANIFEST = "StreamingAssets/Languages/manifest.json";
        private string URL_SCENARII_CATALOG = "StreamingAssets/scenarii/scenariiCatalog.json";

        private string URL_GET_CIDOC_XML = "StreamingAssets/Ontologies/CidocCRM/01-Referential-CidocRDF_Bootleg_GB_3_7_21.xml";
        private string URL_GET_ENTITY_ICONS_COLORS_XML = "StreamingAssets/Ontologies/CidocCRM/01-Referential_Entity_Colour_Mapping.xml";
        private string URL_GET_PROPERTY_ICONS_COLORS_XML = "StreamingAssets/Ontologies/CidocCRM/01-Referential-Property_Colour_mapping.xml";

        //CREATE
        private string URL_CREATE_USER = Path.Combine(HomeUrl, "php/userSave.php");
        private string URL_CREATE_SESSION = Path.Combine(HomeUrl, "php/createsession.php");

        //UPDATE
        private string URL_UPDATE_SESSION = Path.Combine(HomeUrl, "php/updatesession.php");
        private string URL_RESET_PROGRESSION = Path.Combine(HomeUrl, "php/resetprogression.php");

        //DELETE
        private string URL_RESET_GAME = Path.Combine(HomeUrl, "php/resetgame.php?userId={0}");

        //FETCH SCENARII CATALOG
        public async UniTask<ScenariiModelDown> GetCatalog()
        {
            string url = Path.Combine(HomeUrl, URL_SCENARII_CATALOG);
            ScenariiModelDown result = await _net.GetAsync<ScenariiModelDown>(url);
            return result;
        }

        //FETCH LANGUAGE MANIFEST JSON FILE
        public async UniTask<LocalesManifestModel> GetLocalesManifestFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_LOCALES_MANIFEST);

            LocalesManifestModel result = await _net.GetAsync<LocalesManifestModel>(url);

            return result;
        }

        //FETCH COUNTRIES JSON FILE
        public async UniTask<CountriesModelDown> GetCountriesJson(string locale)
        {

            string countriesPath = LanguagesFolder + locale + "/countries/countries.json";

            string url = Path.Combine(HomeUrl, countriesPath);
            CountriesModelDown result = await _net.GetAsync<CountriesModelDown>(url);
            return result;
        }

        //FETCH PLAYER HISTORY
        public async UniTask<HistoryModelDown> GetHistory(int id)
        {
            string url = string.Format(URL_GET_HISTORY, id);

            HistoryModelDown history = await _net.GetAsync<HistoryModelDown>(url);
            return history;
        }

        //FETCH LANGUAGE DICTIONARY FOR LOCALIZATION
        public async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            string localePath = LanguagesFolder + locale;
            string fileNamePath = localePath + "/lang-" + locale + ".json";
            string languagePath = Path.Combine(HomeUrl, fileNamePath);

            var dictionary = await _net.GetAsync<TranslationsModel>(languagePath);
            return dictionary;
        }

        //CHECK STATUS
        public async UniTask<StatusModelDown> CheckStatus(string email)
        {
            string url = string.Format(URL_CHECK_STATUS, email);
            StatusModelDown result = await _net.GetAsync<StatusModelDown>(url);
            return result;
        }

        //CHECK USERNAME
        public async UniTask<ExistValidationDown> CheckUsername(string username)
        {
            string url = string.Format(URL_CHECK_USERNAME, username);
            ExistValidationDown result = await _net.GetAsync<ExistValidationDown>(url);
            return result;
        }

        //GET USERNAME
        public async UniTask<UserNameModelDown> GetUsername(string email)
        {
            string url = string.Format(URL_GET_USERNAME, email);
            UserNameModelDown result = await _net.GetAsync<UserNameModelDown>(url);
            return result;
        }

        //GET USERID
        public async UniTask<UserIdModelDown> GetUserId(string email)
        {
            string url = string.Format(URL_GET_USERID, email);
            UserIdModelDown result = await _net.GetAsync<UserIdModelDown>(url);
            return result;
        }

        //CHECK EMAIL
        public async UniTask<ExistValidationDown> CheckEmail(string email)
        {
            string url = string.Format(URL_CHECK_EMAIL, email);
            ExistValidationDown result = await _net.GetAsync<ExistValidationDown>(url);
            return result;
        }

        //REGISTER
        public async UniTask<SignupModelDown> Register(SignupModelUp formData)
        {
            SignupModelDown result = await _net.PostAsync<SignupModelDown>(URL_CREATE_USER, formData);
            return result;
        }

        //TODO CHECK AND REMOVE ChapterName
        public async UniTask<List<ChallengeData>> LoadChapter(string scenarioName, string chapterName, string fileName)
        {
            string chapterUrl = HomeUrl + "StreamingAssets/scenarii" + "/" + scenarioName + "/Chapters/" + fileName;

            List<ChallengeData> result = await _net.GetAsync<List<ChallengeData>>(chapterUrl);
            return result;
        }

        //LOGIN
        public async UniTask<SigninModelDown> Login(SigninModelUp formData)
        {
            SigninModelDown result = await _net.PostAsync<SigninModelDown>(URL_LOGIN, formData);
            return result;
        }

        //ACTIVATION CODE
        public async UniTask<ActivationCodeModelDown> PostActivationCode(ActivationCodeModelUp email)
        {
            ActivationCodeModelDown code = await _net.PostAsync<ActivationCodeModelDown>(URL_GET_ACTIVATION_CODE, email);
            return code;
        }

        //SEND RESET PASSWORD EMAIL
        public async UniTask<ResetPasswordModelDown> SendResetEmail(ResetPasswordModelUp data)
        {
            ResetPasswordModelDown emailSent = await _net.PostAsync<ResetPasswordModelDown>(URL_SEND_RESET_EMAIL, data);
            return emailSent;
        }

        //Get CIDOC XML files from server
        public async UniTask<string> GetXmlCidocFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_CIDOC_XML);
            string result = await _net.GetRawStringAsync<XmlStringModelDown>(url);
            return result;
        }

        //Get ENTITY COLOR MAPPING XML files from server
        public async UniTask<string> GetXmlEntityIconsColorsFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_ENTITY_ICONS_COLORS_XML);
            string result = await _net.GetRawStringAsync<XmlStringModelDown>(url);
            return result;
        }

        //Get ENTITY COLOR MAPPING XML files from server
        public async UniTask<string> GetXmlPropertyColorsFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_PROPERTY_ICONS_COLORS_XML);
            string result = await _net.GetRawStringAsync<XmlStringModelDown>(url);
            return result;
        }

        //Get INSTANCE file from scenario folder
        public async UniTask<List<InstanceCardModelDown>> GetInstanceFile(string scenarioName)
        {
            string url = HomeUrl + "StreamingAssets/scenarii/" + scenarioName + "/Instances/Instances.json";
            List<InstanceCardModelDown> result = await _net.GetAsync<List<InstanceCardModelDown>>(url);
            return result;
        }

        public async UniTask<bool> GetSession(int UserId, string scenarioName, string chapterName)
        {
            string url = string.Format(URL_GET_SESSION, UserId, scenarioName, chapterName);
            ExistValidationDown doesExist = await _net.GetAsync<ExistValidationDown>(url);
            return doesExist.DoesExist;
        }

        //Get completion of a scenario
        public async UniTask<ProgressionModelDown> GetChapterProgression(int UserId, string scenarioName, string chapterFileName)
        {
            string url = string.Format(URL_GET_PROGRESSION, UserId, scenarioName, chapterFileName);
            ProgressionModelDown progression = await _net.GetAsync<ProgressionModelDown>(url);
            return progression;
        }

        public async UniTask<List<ChapterProgressionModelDown>> GetUserStats(int userId)
        {
            string url = string.Format(URL_GET_USER_STATS, userId);
            List<ChapterProgressionModelDown> userProgressions = await _net.GetAsync<List<ChapterProgressionModelDown>>(url);
            return userProgressions;
        }

        public async UniTask<RankingModelDown> GetRankings()
        {
            string url = string.Format(URL_GET_RANKINGS);
            RankingModelDown rankings = await _net.GetAsync<RankingModelDown>(url);
            return rankings;
        }

        public async UniTask<bool> ResetGame(int userId)
        {
            string url = string.Format(URL_RESET_GAME, userId);
            ExistValidationDown result = await _net.GetAsync<ExistValidationDown> (url);
            return result.DoesExist;
        }

        //////////////////////////                UPDATES                //////////////////////////////////

        //Create new Session
        public async UniTask<bool> CreateSession(int UserId, string scenarioName, string chapterName)
        {
            CreateSessionModelUp session = new CreateSessionModelUp {
                UserId = UserId,
                ScenarioTitle = scenarioName,
                ChapterTitle = chapterName
            };

            ExistValidationDown doesExist = await _net.PostAsync<ExistValidationDown>(URL_CREATE_SESSION, session);
            return doesExist.DoesExist;
        }

        public async UniTask<bool> UpdateSession(UpdateSessionModelUp sessionData)
        {
            ExistValidationDown doesExist = await _net.PostAsync<ExistValidationDown>(URL_UPDATE_SESSION, sessionData);
            return doesExist.DoesExist;
        }

        public async UniTask<bool> ResetProgression(ResetProgressionModelUp sessionData)
        {
            ExistValidationDown doesExist = await _net.PostAsync<ExistValidationDown>(URL_RESET_PROGRESSION, sessionData);
            return doesExist.DoesExist;
        }
    }
}