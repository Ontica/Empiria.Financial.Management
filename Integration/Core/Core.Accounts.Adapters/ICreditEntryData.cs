/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : ICreditEntryData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface for a credit account entry used to connect with external systems.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Adapters {

  /// <summary>Interface for a credit account entry used to connect with external systems.</summary>
  public interface ICreditEntryData {

    string AccountNo {
      get;
    }

    string SubledgerAccountNo {
      get;
    }

    DateTime ApplicationDate {
      get;
    }

    string OperationTypeNo {
      get;
    }

    string OperationName {
      get;
    }

    decimal Amount {
      get;
    }

  }  // ICreditEntryData

}  // Empiria.Financial.Adapters
