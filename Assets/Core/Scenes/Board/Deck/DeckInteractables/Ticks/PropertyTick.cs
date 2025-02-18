#define TRACE_ON
using UnityEngine;
using System;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Board.Deck.DeckInteractables.Ticks
{
    public class PropertyTick : MonoBehaviour
    {
        public enum TickColor
        {
            White,
            Blue,
            Brown,
            Yellow,
            Pink,
            Green,
            BabyBlue,
            Grey,
            Purple
        }

        public enum TickType
        {
            Domain,
            Range
        }

        public bool IsTicked { get => _ticked; }
        public TickType TypeOfTick { get => _type; }
        public TickColor ColorOfTick { get => _tickColor; }

        public event Action<GameObject, TickColor> OnPropertyTickClicked;

        //Fields
        [SerializeField]
        private GameObject _tick;//Attached Tick object
        [SerializeField]
        private TickType _type;
        [SerializeField]
        private TickColor _tickColor;//Color of the tick from the enum above.



        private bool _ticked = false;
        private float _translateUnits;//When un/ticked, the tick is translated of 'translateValue' Unity distance units
        private readonly float _translateValue = 12.0f;

        //Initialize translate offset depending on side of the card the ticks are
        //Domain ticks -> 'Off' is on the right
        //Range ticks -> 'Off' is on the left
        public void Init()
        {
            //Add listener to button of Tick object
            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().onClick.AddListener(Clicked);
            }
            else
            {
                throw new ArgumentNullException("PropertyTick object not found.");
            }

            //Set offset for tick On/Off
            if (_type == TickType.Domain)
            {
                _translateUnits = _translateValue;
            }
            else
            {
                _translateUnits = -_translateValue;
            }
        }

        private void Clicked()
        {
            Trace.Log(string.Format("[PropertyTick] PropertyTick cliked : {0}", _tickColor));
            OnPropertyTickClicked?.Invoke(_tick, _tickColor);
        }

        //UI of tick, set it 'Off' or 'On'
        public void TickOn()
        {
            if (IsTicked == false)
            {
                transform.parent.Translate(0.0f, -_translateUnits, 0.0f);
                _ticked = true;
                Trace.Log(string.Format("[PropertyTick] Tick {0} is ON", _tickColor));
            }
        }

        public void TickOff()
        {
            if (IsTicked == true)
            {
                transform.parent.Translate(0.0f, _translateUnits, 0.0f);
                _ticked = false;
                Trace.Log(string.Format("[PropertyTick] Tick {0} is OFF", _tickColor));
            }
        }
    }
}