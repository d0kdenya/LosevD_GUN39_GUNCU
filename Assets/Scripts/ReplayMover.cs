using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			//todo comment: зачем нужны эти проверки?
			// Проверка на существование компонента и кол-во записей. При несоответствии - ошибка
			if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				// Чтобы отключить работу Update при некорректных данных
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
         //todo comment: Что проверяет это условие (с какой целью)? 
         // Проверяет, наступило ли время следующей сохраненной точки; если да — переходим к следующему сегменту
         if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
				//todo comment: Для чего нужна эта проверка?
				// Для предотвращения обращения к несуществующему индексу. Завершение работы
				if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
			//todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
			// Коэффициент, который показывает насколько мы продвинулись к записанной точке
			var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
			//todo comment: Зачем нужна эта проверка?
			// Проверка на 0 / 0 = NaN
			if (float.IsNaN(delta)) delta = 0f;
         //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
         // Интерполяционно двигаем объект (меняем его позицию через код)
         // Возвращает промежуточную позицию между предыдущей и текущей точками.
			// При delta = 0 это _prev.Position, при delta = 1 — curr.Position, значения между 0 и 1 дают плавное перемещение между ними
         transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}