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
            placeholderMessage = "������� �� ������� ��� �����";

            spaceButtonLabel = "������";

            enterButtonLabel = "����";

            cancelButtonLabel = "������";

            shiftButtonLabel = "Shift";

            backspaceButtonLabel = "�������";

            clearButtonLabel = "��������";

            row1Keys = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-" };

            row1Shift = new string[] { "\"", "'", "(", "-", "_", ")", "=", "+", "*", "/", "." };

            row2Keys = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�" };

            row2Shift = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�" };

            row3Keys = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�" };

            row3Shift = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�" };

            row4Keys = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "." };

            row4Shift = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "/" };

            row1Offset = 0.16f;

            row2Offset = 0.08f;

            row3Offset = 0f;

            row4Offset = -0.08f;
        }
    }
}

