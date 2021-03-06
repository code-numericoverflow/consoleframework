using System;
using System.Collections.Generic;
using System.ComponentModel;
using Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.Binding
{
    [TestClass]
    public class ValidationTest
    {
        class TargetClass : INotifyPropertyChanged
        {
            public String TargetStr {
                get { return targetStr; }
                set {
                    if ( targetStr != value ) {
                        targetStr = value;
                        raisePropertyChanged( "TargetStr" );
                    }
                }
            }

            private string targetStr;

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void raisePropertyChanged( string propertyName ) {
                PropertyChangedEventHandler handler = PropertyChanged;
                if ( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        class SourceClass : INotifyPropertyChanged
        {
            public int SourceInt
            {
                get { return sourceInt; }
                set
                {
                    if (value != sourceInt)
                    {
                        sourceInt = value;
                        raisePropertyChanged("SourceInt");
                    }
                }
            }

            private int sourceInt;

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void raisePropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if ( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            SourceClass source = new SourceClass(  );
            TargetClass target = new TargetClass(  );
            BindingBase binding = new BindingBase( target, "TargetStr", source, "SourceInt" );
            BindingResult lastResult = null;
            binding.OnBinding += result => {
                lastResult = result;
            };
            binding.Bind(  );
            target.TargetStr = "5";
            Assert.IsTrue( source.SourceInt == 5 );
            target.TargetStr = "invalid int";
            Assert.IsTrue(source.SourceInt == 0);
            Assert.IsTrue( lastResult.hasConversionError );
        }
    }
}
