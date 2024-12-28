/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payables Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentInstructionData                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for PaymentInstruction and PaymentLog data.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Payments.Processor.Data {

  /// <summary>Provides data read and write methods for PaymentInstruction and PaymentLog data.</summary>
  static internal class PaymentInstructionData {

    #region Methods

    static internal FixedList<PaymentInstructionLogEntry> GetPaymentInstructionLogs(PaymentInstruction paymentInstruction) {
      Assertion.Require(paymentInstruction, nameof(paymentInstruction));

      var sql = $"SELECT * FROM FMS_PAYMENTS_LOG " +
                $"WHERE PYMT_LOG_PYMT_INSTRUCTION_ID = {paymentInstruction.Id} " +
                $"ORDER BY PYMT_LOG_ID";

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentInstructionLogEntry>(dataOperation);
    }


    static internal void WritePaymentInstruction(PaymentInstruction o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Payment_Instruction",
                     o.Id, o.UID, o.GetEmpiriaType().Id, o.PaymentInstructionNo,
                     o.PaymentOrder.Id, o.Description, o.Broker.Id,
                     extensionData, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePaymentLog(PaymentInstructionLogEntry o, string extensionData) {
      var op = DataOperation.Parse("apd_FMS_Payment_Log",
                     o.Id, o.UID, o.PaymentInstruction.Id,o.ExternalResultText,o.ExternalRequestID,
                     o.RequestTime, o.ApplicationTime,
                     o.RecordingTime, extensionData, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class PaymentInstructionData

}  // namespace Empiria.Payments.Processor.Data
