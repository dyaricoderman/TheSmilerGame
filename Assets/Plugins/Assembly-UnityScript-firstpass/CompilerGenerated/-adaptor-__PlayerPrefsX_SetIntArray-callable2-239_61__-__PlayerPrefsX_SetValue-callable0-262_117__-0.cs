using System;
using System.Collections;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_SetIntArray_0024callable2_0024239_61___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00240
	{
		protected __PlayerPrefsX_SetIntArray_0024callable2_0024239_61__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_SetIntArray_0024callable2_0024239_61___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00240(__PlayerPrefsX_SetIntArray_0024callable2_0024239_61__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1, int arg2)
		{
			_0024from((int[])arg0, arg1, arg2);
		}

		public static __PlayerPrefsX_SetValue_0024callable0_0024262_117__ Adapt(__PlayerPrefsX_SetIntArray_0024callable2_0024239_61__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_SetIntArray_0024callable2_0024239_61___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00240(from).Invoke;
		}
	}
}
