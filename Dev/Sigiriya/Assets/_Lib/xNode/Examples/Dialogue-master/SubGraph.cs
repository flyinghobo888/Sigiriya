using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BasicNodes {
	public class SubGraph : XNode.Node {
		[Input] public bool exec;
		public NodeGraph subGraph;
		[Output] public bool output;
	}
}