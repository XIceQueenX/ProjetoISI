//
//  LoginView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 26/12/2025.
//


import SwiftUI

struct LoginView: View {
  @State private var email = ""
  @State private var password = ""
  @EnvironmentObject var authVM: AuthViewModel
  
  var body: some View {
    NavigationStack {
      VStack(spacing: 20) {
        Text("Login")
          .font(.largeTitle)
          .bold()
        
        TextField("Email", text: $email)
          .keyboardType(.emailAddress)
          .autocapitalization(.none)
          .textFieldStyle(.roundedBorder)
        
        SecureField("Password", text: $password)
          .textFieldStyle(.roundedBorder)
        
        if let error = authVM.errorMessage {
          Text(error)
            .foregroundColor(.red)
            .multilineTextAlignment(.center)
            .padding(.horizontal)
        }
        
        if authVM.isLoading {
          ProgressView()
            .progressViewStyle(CircularProgressViewStyle())
            .padding()
        } else {
          Button(action: {
            Task {
              await authVM.login(email: email, password: password)
            }
          }) {
            Text("Login")
              .frame(maxWidth: .infinity)
          }
          .buttonStyle(.borderedProminent)
          .disabled(email.isEmpty || password.isEmpty)
        }
        
        Spacer()
      }
      .padding()
      
      .navigationDestination(isPresented: $authVM.isAuthenticated) {
        MainTabView()
          .environmentObject(authVM)
      }
    }
  }
}
#Preview {
  let authVM = AuthViewModel()
  LoginView()
    .environmentObject(authVM)
}
