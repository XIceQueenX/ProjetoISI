//
//  TrabalhoISIApp.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//

import SwiftUI

@main
struct TrabalhoISIApp: App {
    @StateObject private var authVM = AuthViewModel()
    var body: some Scene {
      WindowGroup {
                  if authVM.isAuthenticated {
                      MainTabView()
                          .environmentObject(authVM)
                  } else {
                      LoginView()
                          .environmentObject(authVM)
                  }
              }
    }
}
