/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Adaptation Interface                    *
*  Type     : ICreditAccountData                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with credit account data used to connect with external systems.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with credit account data used to connect with external systems.</summary>
  public interface ICreditAccountData {

    string AccountNo {
      get;
    }

    string CustomerName {
      get;
    }

    string CustomerNo {
      get;
    }

    string SubledgerAccountNo {
      get;
    }

  }  // interface ICreditAccountData

} // namespace Empiria.Financial.Adapters
