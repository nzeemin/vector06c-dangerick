// Decompiled with JetBrains decompiler
// Type: GulImage.Properties.Resources
// Assembly: GulImage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 367B3184-2483-40A5-B8C3-9858C62B2ADB
// Assembly location: D:\Work\MyProjects-old\vector06c-rick\lviv\src\Resources\GulImage.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GulImage.Properties
{
    [CompilerGenerated]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals((object)GulImage.Properties.Resources.resourceMan, (object)null))
                    GulImage.Properties.Resources.resourceMan = new ResourceManager("GulImage.Properties.Resources", typeof(GulImage.Properties.Resources).Assembly);
                return GulImage.Properties.Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return GulImage.Properties.Resources.resourceCulture;
            }
            set
            {
                GulImage.Properties.Resources.resourceCulture = value;
            }
        }

        internal static string pal
        {
            get
            {
                return GulImage.Properties.Resources.ResourceManager.GetString(nameof(pal), GulImage.Properties.Resources.resourceCulture);
            }
        }
    }
}
