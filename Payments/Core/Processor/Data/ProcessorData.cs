/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payables Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : ProcessorData                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for Proccesor (Payment Instruccion,                       *
*             Successful and Rejected Payment).                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Data;

namespace Empiria.Payments.Processor.Data {

  /// <summary>Provides data read and write methods for Proccesor (Payment Instruccion,                       
  ///          Successful and Rejected Payment).                      </summary>
  static internal class ProcessorData {

    #region Methods
     

    static internal void WritePaymentInstruction(PaymentInstruction o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payment_Instruction",
                     o.Id, o.UID, o.TypeId, o.PaymentOrder.Id, o.Description, o.Broker.Id,
                     extensionData, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePaymentLog(PaymentLog o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payment_Log",
                     o.Id, o.UID, 1, o.RequestTime, o.ApplicationTime, 
                     o.RecordingTime, extensionData, (char) o.Status);

      DataWriter.Execute(op);
    }


    #endregion Methods

  }  // class  ProcessorData

}  // namespace Empiria.Payments.Processor.Data
