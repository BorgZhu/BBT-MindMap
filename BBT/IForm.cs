﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace BBT
{
    /// <summary>
    /// Interface für die Forms
    /// </summary>
    public interface IForm
    {
        Grid getStrokeFromNode(ANode node);
    }
}
