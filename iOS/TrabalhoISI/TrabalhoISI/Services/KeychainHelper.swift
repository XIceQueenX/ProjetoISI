//
//  KeychainHelper.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation
import Security
import Foundation
import Security

class KeychainHelper {
  static let shared = KeychainHelper()
  
  private let service = "com.booksmovies.app"
  private let account = "authToken"
  private let roleAccount = "userRole"
  
  private init() {}
  
  // Save token + role
  func saveToken(_ token: String, role: String? = nil) {
    let tokenData = Data(token.utf8)
    
    // Save token
    let tokenQuery: [String: Any] = [
      kSecClass as String: kSecClassGenericPassword,
      kSecAttrService as String: service,
      kSecAttrAccount as String: account,
      kSecValueData as String: tokenData
    ]
    SecItemDelete(tokenQuery as CFDictionary)
    SecItemAdd(tokenQuery as CFDictionary, nil)
    
    // Save role if provided
    if let role = role {
      let roleData = Data(role.utf8)
      let roleQuery: [String: Any] = [
        kSecClass as String: kSecClassGenericPassword,
        kSecAttrService as String: service,
        kSecAttrAccount as String: roleAccount,
        kSecValueData as String: roleData
      ]
      SecItemDelete(roleQuery as CFDictionary)
      SecItemAdd(roleQuery as CFDictionary, nil)
    }
  }
  
  func getToken() -> String? {
    let query: [String: Any] = [
      kSecClass as String: kSecClassGenericPassword,
      kSecAttrService as String: service,
      kSecAttrAccount as String: account,
      kSecReturnData as String: true
    ]
    
    var result: AnyObject?
    SecItemCopyMatching(query as CFDictionary, &result)
    
    guard let data = result as? Data else { return nil }
    return String(data: data, encoding: .utf8)
  }
  
  func getUserRole() -> String? {
    let query: [String: Any] = [
      kSecClass as String: kSecClassGenericPassword,
      kSecAttrService as String: service,
      kSecAttrAccount as String: roleAccount,
      kSecReturnData as String: true
    ]
    
    var result: AnyObject?
    SecItemCopyMatching(query as CFDictionary, &result)
    
    guard let data = result as? Data else { return nil }
    return String(data: data, encoding: .utf8)
  }
  
  func deleteTokenAndRole() {
    let tokenQuery: [String: Any] = [
      kSecClass as String: kSecClassGenericPassword,
      kSecAttrService as String: service,
      kSecAttrAccount as String: account
    ]
    SecItemDelete(tokenQuery as CFDictionary)
    
    let roleQuery: [String: Any] = [
      kSecClass as String: kSecClassGenericPassword,
      kSecAttrService as String: service,
      kSecAttrAccount as String: roleAccount
    ]
    SecItemDelete(roleQuery as CFDictionary)
  }
}
