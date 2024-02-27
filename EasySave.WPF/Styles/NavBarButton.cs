using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace EasySave.Styles
{
    public class NavBarButton : RadioButton
    {
        static NavBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavBarButton), new FrameworkPropertyMetadata(typeof(NavBarButton)));
        }
    }
}
