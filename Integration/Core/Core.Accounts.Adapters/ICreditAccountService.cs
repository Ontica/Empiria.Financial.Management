/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Adaptation Interface                    *
*  Type     : ICreditAccountService                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface used to retrieve credit accounts data from external systems.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Interface used to retrieve credit accounts data from external systems.</summary>
  public interface ICreditAccountService {

    ICreditAccountData TryGetCredit(string creditNo);

  }  // ICreditAccountService

}  // namespace Empiria.Financial.Adapters
