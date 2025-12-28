//
//  AuthViewModel.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation
import SwiftUI
import Combine

@MainActor
class AuthViewModel: ObservableObject {
  @Published var isAuthenticated = false
  @Published var currentUser: User?
  @Published var isLoading = false
  @Published var errorMessage: String?
  
  init() {
    checkAuthentication()
  }
  
  func checkAuthentication() {
    isAuthenticated = KeychainHelper.shared.getToken() != nil
  }
  
  func login(email: String, password: String) async {
    isLoading = true
    errorMessage = nil
    
    do {
      let response = try await AuthService.shared.login(email: email, password: password)
      
      KeychainHelper.shared.saveToken(response.token)
      
      currentUser = User(id: 0, username: response.username, email: "", role: "")
      isAuthenticated = true
    } catch {
      errorMessage = "Login failed: \(error.localizedDescription)"
    }
    
    isLoading = false
  }
  
  
  func register(username: String, email: String, password: String) async {
    isLoading = true
    errorMessage = nil
    
    do {
      let response = try await AuthService.shared.register(username: username, email: email, password: password)
      
      KeychainHelper.shared.saveToken(response.token)
      
      currentUser = User(
        id: 0,
        username: response.username,
        email: email,
        role: ""
      )
      
      isAuthenticated = true
      
    } catch {
      errorMessage = "Registration failed: \(error.localizedDescription)"
    }
    
    isLoading = false
  }
  
  
  func logout() {
    AuthService.shared.logout()
    currentUser = nil
    isAuthenticated = false
  }
}
