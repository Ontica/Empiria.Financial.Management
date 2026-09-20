/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                            Component : Adapters Layer                     *
*  Assembly : Empiria.Financial.Integration.Core.dll          Pattern   : Fields DTO                         *
*  Type     : FinancialTransactionFields                      License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Fields DTO holder for financial transactions.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Financial.Transactions.Adapters {

  /// <summary>Fields DTO holder for financial transactions.</summary>
  public class FinancialTransactionFields {

    public string TransactionKey {
      get; set;
    }

    public string Description {
      get; set;
    }

    public int TransactionReferenceId {
      get; set;
    }

    public int TraceableEntityReferenceId {
      get; set;
    }

    public string SourceCode {
      get; set;
    }

    public DateTime ApplicationDate {
      get; set;
    }

    public DateTime RecordingTime {
      get; set;
    }

    public JsonObject Payload {
      get; set;
    }


    public void EnsureValid() {
      Assertion.Require(TransactionKey, nameof(TransactionKey));

      Assertion.Require(Description, nameof(Description));

      Assertion.Require(TransactionReferenceId != 0 &&
                        TraceableEntityReferenceId != -1, nameof(TransactionReferenceId));

      Assertion.Require(SourceCode, nameof(SourceCode));

      Assertion.Require(ApplicationDate, nameof(ApplicationDate));
      Assertion.Require(RecordingTime, nameof(RecordingTime));
      Assertion.Require(ApplicationDate <= RecordingTime, "Application date must be less than or equal to recording time.");

      Assertion.Require(Payload, nameof(Payload));
      Assertion.Require(Payload.HasItems,
                        "Financial transaction payload must not be empty.");
    }

  }  // class FinancialTransactionFields

}  // namespace Empiria.Financial.Transactions.Adapters
