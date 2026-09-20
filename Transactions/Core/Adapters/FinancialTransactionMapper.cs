/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Transactions                        Component : Adapters Layer                       *
*  Assembly : Empiria.Financial.Transactions.Core.dll       Pattern   : Output DTO                           *
*  Type     : FinancialTransactionMapper                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Services used to map FinancialTransaction objects to a FinancialTransactionDto.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Transactions.Adapters {

  /// <summary>Service used for map FinancialTransaction objects to a FinancialTransactionDto.</summary>
  static internal class FinancialTransactionMapper {

    static internal FinancialTransactionDto Map(FinancialTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      return new FinancialTransactionDto {
        TransactionKey = transaction.TransactionKey,
        Description = transaction.Description,
        TransactionReferenceId = transaction.TransactionReferenceId,
        TraceableEntityReferenceId = transaction.TraceableEntityReferenceId,
        SourceCode = transaction.SourceCode,
        ApplicationDate = transaction.ApplicationDate,
        RecordingTime = transaction.RecordingTime,
        Payload = transaction.Payload
      };
    }

  } // class FinancialTransactionDto

}  // namespace Empiria.Financial.Transactions.Adapters
