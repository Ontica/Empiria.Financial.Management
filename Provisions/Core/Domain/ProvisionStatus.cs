/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Provisions Management                      Component : Domain Layer                            *
*  Assembly : Empiria.Provisions.Core.dll                Pattern   : Enumeration Type                        *
*  Type     : ProvisionStatus                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a provision.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

/// <summary>Enumerates the status of a provision.</summary>
public enum ProvisionStatus {

  Programmed = 'G',

  Provisioned = 'V',

  Canceled = 'L',

  All = '@'

}  // enum ProvisionStatus
