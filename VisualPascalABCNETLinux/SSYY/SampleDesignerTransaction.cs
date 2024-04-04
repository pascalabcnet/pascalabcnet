//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
namespace SampleDesignerHost
{
    using System;
	using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics;
    using System.Collections;
    using System.Windows.Forms;
    using System.Reflection;
    using System.Text;
    
	/// Designer transactions offer a mechanism to improve performance by wrapping around
	/// a series of component changes. The changes are not actually committed until the
	/// entire batch is processed. They can be aborted if the transaction is canceled.
    internal class SampleDesignerTransaction : DesignerTransaction {
        private SampleDesignerHost host;

        public SampleDesignerTransaction(SampleDesignerHost host, string description) : base(description) {
            this.host = host;

			// The host keeps a string stack of the transaction descriptions.
            host.TransactionDescriptions.Push(description);

			// If this is first transaction to be opened, have the host raise
			// opening/opened events.
			//
            if (host.TransactionCount++ == 0) {
                host.OnTransactionOpening(EventArgs.Empty);
                host.OnTransactionOpened(EventArgs.Empty);
            }
        }

        protected override void OnCancel() {
            if (host != null) {
                Debug.Assert(host.TransactionDescriptions != null, "End batch operation with no desription?!?");
                string s =  (string)host.TransactionDescriptions.Pop();

				// If this is the last transaction to be closed, have the host raise
				// closing/closed events.
				//
                if (--host.TransactionCount == 0) {
                    DesignerTransactionCloseEventArgs dtc = new DesignerTransactionCloseEventArgs(false);
                    host.OnTransactionClosing(dtc);
                    host.OnTransactionClosed(dtc);
                }
                host = null;
            }
        }

        protected override void OnCommit() {
            if (host != null) {
                Debug.Assert(host.TransactionDescriptions != null, "End batch operation with no desription?!?");
                string s =  (string)host.TransactionDescriptions.Pop();

				// If this is the last transaction to be closed, have the host raise
				// closing/closed events.
				//
                if (--host.TransactionCount == 0) 
				{
                    DesignerTransactionCloseEventArgs dtc = new DesignerTransactionCloseEventArgs(true);
                    host.OnTransactionClosing(dtc);
                    host.OnTransactionClosed(dtc);
                }
                host = null;
            }
        }
    }
}
