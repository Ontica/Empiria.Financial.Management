/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Transactions                          Component : Domain Layer                       *
*  Assembly : Empiria.Financial.Transactions.Core.dll         Pattern   : Information Holder                 *
*  Type     : FinancialTransaction                            License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Holds information related to a financial transaction.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;

namespace Empiria.Financial.Transactions {

  /// <summary>Holds information related to a financial transaction.</summary>
  public class FinancialTransaction {

    public FinancialTransaction(string transactionKey,
                                IIdentifiable transaction, IIdentifiable traceableEntity,
                                OperationSource source, DateTime transactionTime,
                                DateTime recordingTime, JsonObject payload) {

      Assertion.Require(transactionKey, nameof(transactionKey));
      Assertion.Require(transaction, nameof(transaction));

      Assertion.Require(traceableEntity, nameof(traceableEntity));

      Assertion.Require(source, nameof(source));

      Assertion.Require(transactionTime <= DateTime.Now, nameof(transactionTime));
      Assertion.Require(recordingTime <= DateTime.Now, nameof(recordingTime));
      Assertion.Require(transactionTime <= recordingTime,
                        "Recording date must be greater than or equal to transaction date.");

      Assertion.Require(payload, nameof(payload));
      Assertion.Require(payload.HasItems, nameof(payload));

      TransactionKey = transactionKey;
      TransactionReferenceId = transaction.Id;
      TraceableEntityReferenceId = traceableEntity.Id;
      SourceCode = source.Code;
      TransactionTime = transactionTime;
      RecordingTime = recordingTime;
      Payload = payload;
    }

    #region Properties

    public string TransactionKey {
      get;
    }

    public int TransactionReferenceId {
      get;
    }

    public int TraceableEntityReferenceId {
      get;
    }

    public string SourceCode {
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

    #endregion Properties

  }  // class FinancialTransaction

}  // namespace Empiria.Financial.Transactions
