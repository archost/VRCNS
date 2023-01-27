using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

namespace VRKeys.Layouts
{
    public class Russian : Layout
    {
        public Russian()
        {
            placeholderMessage = "Нажмите на клавиши для ввода";

            spaceButtonLabel = "Пробел";

            enterButtonLabel = "Ввод";

            cancelButtonLabel = "Отмена";

            shiftButtonLabel = "Shift";

            backspaceButtonLabel = "Стереть";

            clearButtonLabel = "Очистить";

            row1Keys = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-" };

            row1Shift = new string[] { "\"", "'", "(", "-", "_", ")", "=", "+", "*", "/", "." };

            row2Keys = new string[] { "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ъ" };

            row2Shift = new string[] { "Й", "Ц", "У", "К", "Е", "Н", "Г", "Ш", "Щ", "З", "Х", "Ъ" };

            row3Keys = new string[] { "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э" };

            row3Shift = new string[] { "Ф", "Ы", "В", "А", "П", "Р", "О", "Л", "Д", "Ж", "Э" };

            row4Keys = new string[] { "я", "ч", "с", "м", "и", "т", "ь", "б", "ю", "." };

            row4Shift = new string[] { "Я", "Ч", "С", "М", "И", "Т", "Ь", "Б", "Ю", "/" };

            row1Offset = 0.16f;

            row2Offset = 0.08f;

            row3Offset = 0f;

            row4Offset = -0.08f;
        }
    }
}

