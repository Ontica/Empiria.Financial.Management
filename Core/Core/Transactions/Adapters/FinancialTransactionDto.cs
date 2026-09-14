/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Transactions                          Component : Adapters Layer                     *
*  Assembly : Empiria.Financial.Core.dll                      Pattern   : Ouput DTO                          *
*  Type     : FinancialTransactionDto                         License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Output DTO with data related to a financial transaction.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;

namespace Empiria.Financial.Transactions.Adapters {

  /// <summary>Output DTO holder for financial transactions.</summary>
  public class FinancialTransactionDto {

    public FinancialTransactionDto(string transactionCode,
                                   IIdentifiable transaction, IIdentifiable traceableObject,
                                   OperationSource source, DateTime transactionTime,
                                   DateTime recordingTime, JsonObject payload) {

      Assertion.Require(transactionCode, nameof(transactionCode));
      Assertion.Require(transaction, nameof(transaction));

      Assertion.Require(traceableObject, nameof(traceableObject));

      Assertion.Require(source, nameof(source));

      Assertion.Require(transactionTime <= DateTime.Now, nameof(transactionTime));
      Assertion.Require(recordingTime <= DateTime.Now, nameof(recordingTime));
      Assertion.Require(transactionTime <= recordingTime,
                        "Recording date must be greater than or equal to transaction date.");

      Assertion.Require(payload, nameof(payload));
      Assertion.Require(payload.HasItems, nameof(payload));

      TransactionCode = transactionCode;
      TransactionId = transaction.Id;
      TraceableObjectId = traceableObject.Id;
      SourceId = source.Id;
      TransactionTime = transactionTime;
      RecordingTime = recordingTime;
      Payload = payload;
    }


    public string TransactionCode {
      get;
    }

    public int TransactionId {
      get;
    }

    public int TraceableObjectId {
      get;
    }

    public int SourceId {
      get;
    }

    public DateTime TransactionTime {
      get;
    }

    public DateTime RecordingTime {
      get;
    }

    public JsonObject Payload {
      get;
    }

  }  // class FinancialTransactionDto

}  // namespace Empiria.Financial.Transactions.Adapters
