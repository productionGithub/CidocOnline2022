using TMPro;
using UnityEngine;

using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.Stats
{
    public class ScenarioPanelEntry : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scenarioTitle;
        [SerializeField] private ChapterLineDetailEntry _chapterLineTemplate;
        [SerializeField] private Transform _detailContainer;

        [SerializeField] TextMeshProUGUI _scenarioTotalProgression;
        [SerializeField] TextMeshProUGUI _scenarioTotalScore;
        [SerializeField] TextMeshProUGUI _scenarioMaxPossibleScore;

        private List<ChapterLineDetailEntry> _entriesList;

        public int totalScenarioProgression;
        public int totalScenarioScore;
        public int totalScenarioMaxPossibleScore;
        public int nbChapterInScenario;

        public void Init()
        {
            if (_entriesList == null)
            {
                _entriesList = new List<ChapterLineDetailEntry>();
            }
            else
            {
                foreach (ChapterLineDetailEntry e in _entriesList)
                {
                    Destroy(e.gameObject);
                }
                _entriesList.Clear();
            }
            _chapterLineTemplate.gameObject.SetActive(false);
        }

        public void Show(string title, List<ChapterProgressionModelDown> chapterEntries)
        {
            totalScenarioProgression = 0;
            totalScenarioScore = 0;
            totalScenarioMaxPossibleScore = 0;
            nbChapterInScenario = 0;

            float _totalChapterProgression;
        
            _scenarioTitle.text = title;

            foreach (ChapterProgressionModelDown chap in chapterEntries)
            {
                _totalChapterProgression = 0;

                if (chap.ScenarioName.Equals(title))
                {
                    ChapterLineDetailEntry chapLineInstance = Instantiate(_chapterLineTemplate, _detailContainer);
                    _entriesList.Add(chapLineInstance);
                    chapLineInstance.gameObject.SetActive(true);

                    if (chap.LastChallengeId == 1)
                    {
                        //totalScenarioProgression = 0;
                        _totalChapterProgression = 0;
                    }
                    else
                    {
                        _totalChapterProgression += (chap.LastChallengeId * 100) / chap.MaxChallengeCount;

                        //totalScenarioProgression += (chap.LastChallengeId * 100) / chap.MaxChallengeCount;
                        Debug.Log("");
                    }

                    totalScenarioProgression += (int) _totalChapterProgression;

                    totalScenarioScore += chap.Score;
                    totalScenarioMaxPossibleScore += chap.MaxPossibleScore;

                    nbChapterInScenario++;

                    chapLineInstance.Show(chap);
                }

                Debug.Log("");

            }
            //_scenarioTotalProgression.text = (totalScenarioProgression * 100) / (nbChapterInScenario * 100) + "%";
            _scenarioTotalProgression.text = totalScenarioProgression / nbChapterInScenario + "%";
            Debug.Log("");
            _scenarioTotalScore.text = totalScenarioScore.ToString();
            _scenarioMaxPossibleScore.text = totalScenarioMaxPossibleScore.ToString();
        }
    }
}
