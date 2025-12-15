/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payables Management                        Component : Data Layer                              *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data service                            *
*  Type     : PaymentInstructionData                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for PaymentInstruction and PaymentLog data.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Payments.Data {

  /// <summary>Provides data read and write methods for PaymentInstruction and PaymentLog data.</summary>
  static internal class PaymentInstructionData {

    #region Methods

    static internal List<PaymentInstructionLogEntry> GetPaymentInstructionLog(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));

      var sql = $"SELECT * FROM FMS_PAYMENTS_LOG " +
                $"WHERE PYMT_LOG_PYMT_INSTRUCTION_ID = {instruction.Id} " +
                $"ORDER BY PYMT_LOG_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<PaymentInstructionLogEntry>(op);
    }


    static internal List<PaymentInstruction> GetPaymentOrderInstructions(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      var sql = $"SELECT * FROM FMS_PAYMENT_INSTRUCTIONS " +
                $"WHERE PYMT_INSTRUCTION_PYMT_ORDER_ID = {paymentOrder.Id} " +
                $"ORDER BY PYMT_INSTRUCTION_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<PaymentInstruction>(op);
    }


    static internal FixedList<PaymentInstruction> GetInProgressPaymentInstructions() {

      var sql = $"SELECT * FROM FMS_PAYMENT_INSTRUCTIONS " +
                $"WHERE PYMT_INSTRUCTION_STATUS = 'A' " +
                $"ORDER BY PYMT_INSTRUCTION_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentInstruction>(op);
    }


    static internal FixedList<PaymentInstructionLogEntry> GetPaymentOrderInstructionLogs(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      var sql = $"SELECT * FROM FMS_PAYMENTS_LOG " +
                $"WHERE PYMT_LOG_PYMT_ORDER_ID = {paymentOrder.Id} " +
                $"ORDER BY PYMT_LOG_ID";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<PaymentInstructionLogEntry>(op);
    }


    static internal void WritePaymentInstruction(PaymentInstruction o) {
      var op = DataOperation.Parse("write_FMS_Payment_Instruction",
                     o.Id, o.UID, o.GetEmpiriaType().Id, o.PaymentInstructionNo,
                     o.PaymentOrder.Id, o.Description, o.BrokerConfigData.Id,
                     o.BrokerInstructionNo, o.ExtData.ToString(),
                     o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WritePaymentLog(PaymentInstructionLogEntry o) {
      var op = DataOperation.Parse("apd_FMS_Payment_Log",
                     o.Id, o.UID, o.PaymentInstruction.Id, o.PaymentOrder.Id,
                     o.BrokerMessage, o.BrokerInstructionNo,
                     o.RequestTime, o.ApplicationTime, o.RecordingTime,
                     o.ExtData.ToString(), (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Methods

  }  // class PaymentInstructionData

}  // namespace Empiria.Payments.Data
