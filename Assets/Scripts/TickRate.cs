using UnityEngine;

namespace Server
{
    public class TickRate : MonoBehaviour
    {
        [Tooltip("Тикрейт (сколько тиков в секунду нужно)")]
        public float tickRate = 20f;

        private float tickInterval;
        private float accumulator = 0f;

        // Событие или метод, который вызывается в каждый тик
        public delegate void OnTickHandler();
        public event OnTickHandler OnTick;


        private void Start()
        {
            // Вычисляем интервал времени на один тик (например, 1 / 20 = 0.05 сек)
            tickInterval = 1f / tickRate;

        }

        private void Update()
        {
            // Прибавляем время, прошедшее с предыдущего кадра
            accumulator += Time.deltaTime;

            // Пока накопилось достаточно времени на один или несколько тиков
            while (accumulator >= tickInterval)
            {
                Tick();
                accumulator -= tickInterval;
            }
        }

        private void Tick()
        {
            // Здесь выполняется логика тика (движение, физика, отправка пакетов и т.д.)
            OnTick?.Invoke();
            // Пример: Debug.Log("Tick executed at: " + Time.time);
        }
    }
}