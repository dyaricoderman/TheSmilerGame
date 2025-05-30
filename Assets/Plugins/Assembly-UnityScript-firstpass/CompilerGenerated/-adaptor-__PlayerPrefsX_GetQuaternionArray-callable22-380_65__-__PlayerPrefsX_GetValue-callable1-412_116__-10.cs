using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002410
	{
		protected __PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002410(__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<Quaternion>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002410(from).Invoke;
		}
	}
}
