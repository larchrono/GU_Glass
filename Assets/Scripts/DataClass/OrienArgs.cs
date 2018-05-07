using System;

public class OrienArgs : EventArgs  {

	public float O_X;
	public float O_Y;
	public float O_Z;

	public OrienArgs(){}

	public OrienArgs(string srcx, string srcy,string srcz){
		float.TryParse (srcx, out this.O_X);
		float.TryParse (srcy, out this.O_Y);
		float.TryParse (srcz, out this.O_Z);
	}
}
