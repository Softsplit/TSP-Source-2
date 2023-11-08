
using System;

namespace TSPS2.Menu;

internal class SliderStepAttribute : Attribute
{
	public readonly float Step;

	public SliderStepAttribute( float step ) => Step = step;
}