using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMoneyComponent: MonoBehaviour
    {
        [SerializeField] private int initialMoney = 40;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI totalMoneyText;
        [SerializeField] private Image moneyBar;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI plusMoney;
        [SerializeField] private TextMeshProUGUI minusMoney;
        [SerializeField] private float accumulationResetTime = 1f;

        [SerializeField] private List<StatusRequirement> statusRequirements;

        private int _money;
        private int _totalMoney;
        private PlayerStatus _status;
        private int _moneyForMaxStatus;

        private int _accumulatedPlusMoney;
        private float _lastPlusTime;
        private Animator _plusAnim;
        private int _accumulatedMinusMoney;
        private float _lastMinusTime;
        private Animator _minusAnim;
        private static readonly int ResetTrigger = Animator.StringToHash("reset");

        private void Start()
        {
            statusRequirements.Sort((a, b) => Math.Sign(a.money - b.money));
            _moneyForMaxStatus = statusRequirements[^1].money;
            _plusAnim = plusMoney.transform.parent.GetComponent<Animator>();
            _minusAnim = minusMoney.transform.parent.GetComponent<Animator>();
            ResetMoney();
        }

        public void ResetMoney(bool success = false)
        {
            if (success)
            {
                _totalMoney += _money;
                totalMoneyText.text = _totalMoney.ToString();
            }
            _money = initialMoney;
            OnMoneyUpdate();
        }

        public void AddMoney(int value)
        {
            _money += value;
            PlayerAudio.Instance.CollectCoins();
            OnMoneyUpdate();

            if (Time.time - _lastPlusTime > accumulationResetTime)
            {
                _accumulatedPlusMoney = 0;
            }
            _accumulatedPlusMoney += value;
            plusMoney.text = $"+{_accumulatedPlusMoney}";
            _lastPlusTime = Time.time;
            _plusAnim.SetTrigger(ResetTrigger);
        }

        public void TakeMoney(int value)
        {
            _money -= value;
            if (_money <= 0)
            {
                _money = 0;
                GameManager.Instance.Fail();
            }
            
            PlayerAudio.Instance.LoseCoins();
            OnMoneyUpdate();

            if (Time.time - _lastMinusTime > accumulationResetTime)
            {
                _accumulatedMinusMoney = 0;
            }
            _accumulatedMinusMoney += value;
            minusMoney.text = $"-{_accumulatedMinusMoney}";
            _lastMinusTime = Time.time;
            _minusAnim.SetTrigger(ResetTrigger);
        }

        private void OnMoneyUpdate()
        {
            var status = CalculateStatus();
            if (status.status != _status)
            {
                _status = status.status;
                ResetAppearance();
                status.appearance.SetActive(true);
                moneyBar.sprite = status.moneyBar;
                statusText.text = status.statusName;
                statusText.color = status.statusColor;
            }
            moneyText.text = _money.ToString();
            moneyBar.fillAmount = _money / (float) _moneyForMaxStatus;
        }

        private StatusRequirement CalculateStatus() => statusRequirements.Last(statusRequirement => _money >= statusRequirement.money);

        private void ResetAppearance()
        {
            foreach (var appearance in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                appearance.gameObject.SetActive(false);
            }
        }
    }
}