/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                            Component : Adapters Layer                     *
*  Assembly : Empiria.Financial.Integration.Core.dll          Pattern   : Output DTO                          *
*  Type     : FinancialTransactionDto                         License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Output DTO holder for financial transactions.<                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;

namespace Empiria.Financial.Transactions.Adapters {

  /// <summary>Output DTO holder for financial transactions.</summary>
  public class FinancialTransactionDto {

    public string TransactionCode {
      get; set;
    }

    public int TransactionId {
      get; set;
    }

    public int TraceableObjectId {
      get; set;
    }

    public int SourceId {
      get; set;
    }

    public DateTime TransactionTime {
      get; set;
    }

    public DateTime RecordingTime {
      get; set;
    }

    public JsonObject Payload {
      get; set;
    }

  }  // class FinancialTransactionDto

}  // namespace Empiria.Financial.Transactions.Adapters
