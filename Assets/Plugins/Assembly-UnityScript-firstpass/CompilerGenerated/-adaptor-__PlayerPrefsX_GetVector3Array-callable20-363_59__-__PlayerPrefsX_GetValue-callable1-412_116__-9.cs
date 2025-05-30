using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00249
	{
		protected __PlayerPrefsX_GetVector3Array_0024callable20_0024363_59__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00249(__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<Vector3>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00249(from).Invoke;
		}
	}
}
