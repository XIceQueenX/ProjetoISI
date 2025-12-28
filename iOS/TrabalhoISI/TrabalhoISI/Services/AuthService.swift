//
//  AuthService.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//


import Foundation

class AuthService {
  static let shared = AuthService()
  
  private let baseURL = "https://trabalho-isi-gdf5d0gnh4bwawfw.westeurope-01.azurewebsites.net/api"
  
  private init() {}
  
  func register(username: String, email: String, password: String) async throws -> AuthResponse {
    guard let url = URL(string: "\(baseURL)/auth/register") else {
      throw URLError(.badURL)
    }
    
    let request = RegisterRequest(username: username, email: email, password: password)
    let encoder = JSONEncoder()
    let body = try encoder.encode(request)
    
    var urlRequest = URLRequest(url: url)
    urlRequest.httpMethod = "POST"
    urlRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")
    urlRequest.httpBody = body
    
    let (data, response) = try await URLSession.shared.data(for: urlRequest)
    
    guard let httpResponse = response as? HTTPURLResponse,
          (200...299).contains(httpResponse.statusCode) else {
      throw URLError(.badServerResponse)
    }
    
    let decoder = JSONDecoder()
    let authResponse = try decoder.decode(AuthResponse.self, from: data)
    
    let role = (authResponse.username.lowercased() == "admin") ? "Admin" : "User"
    KeychainHelper.shared.saveToken(authResponse.token, role: role)
    
    return authResponse
  }
  
  func login(email: String, password: String) async throws -> AuthResponse {
    guard let url = URL(string: "\(baseURL)/auth/login") else {
      throw URLError(.badURL)
    }
    
    let request = LoginRequest(email: email, password: password)
    let encoder = JSONEncoder()
    let body = try encoder.encode(request)
    
    var urlRequest = URLRequest(url: url)
    urlRequest.httpMethod = "POST"
    urlRequest.setValue("application/json", forHTTPHeaderField: "Content-Type")
    urlRequest.httpBody = body
    
    let (data, response) = try await URLSession.shared.data(for: urlRequest)
    
    guard let httpResponse = response as? HTTPURLResponse,
          (200...299).contains(httpResponse.statusCode) else {
      throw URLError(.badServerResponse)
    }
    
    let decoder = JSONDecoder()
    let authResponse = try decoder.decode(AuthResponse.self, from: data)
    
    let role = (authResponse.username.lowercased() == "admin") ? "Admin" : "User"
    KeychainHelper.shared.saveToken(authResponse.token, role: role)
    return authResponse
  }
  
  func logout() {
    KeychainHelper.shared.deleteTokenAndRole()
  }
}
