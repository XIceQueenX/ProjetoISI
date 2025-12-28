//
//  User.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import Foundation

struct User: Codable {
  let id: Int
  let username: String
  let email: String
  let role: String
}

struct LoginRequest: Codable {
  let email: String
  let password: String
}

struct RegisterRequest: Codable {
  let username: String
  let email: String
  let password: String
}

struct AuthResponse: Codable {
  let token: String
  let username: String
  let expiration: String
}

