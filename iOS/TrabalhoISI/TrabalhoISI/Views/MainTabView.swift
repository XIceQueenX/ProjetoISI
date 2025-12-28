//
//  MainTabView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 27/12/2025.
//


import SwiftUI

struct MainTabView: View {
  @EnvironmentObject var authVM: AuthViewModel
  
  var body: some View {
    TabView {
      BooksView()
        .tabItem { Label("Books", systemImage: "book") }
      
      MoviesView()
        .tabItem { Label("Movies", systemImage: "film") }
      
      RecommendationsView()
        .tabItem { Label("Recommendations", systemImage: "star") }
      
      Button("Logout") {
        authVM.logout()
      }
      .tabItem { Label("Logout", systemImage: "person.crop.circle") }
    }
  }
}
