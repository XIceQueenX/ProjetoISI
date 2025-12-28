//
//  RecommendationsView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 27/12/2025.
//
import SwiftUI

struct RecommendationsView: View {
  @StateObject var vm = RecommendationsViewModel()

  var body: some View {
    NavigationStack {
      VStack(spacing: 16) {

        // MARK: - Recommendation Type Picker
        Picker("Recommendation Type", selection: $vm.selectedType) {
          ForEach(RecommendationType.allCases, id: \.self) { type in
            Text(type.title).tag(type)
          }
        }
        .pickerStyle(SegmentedPickerStyle())
        .padding(.horizontal)

        // MARK: - Input Field for Source ID
        TextField(vm.selectedType == .moviesForBook ? "Enter Book ID" : "Enter Movie ID", text: $vm.sourceId)
          .textFieldStyle(RoundedBorderTextFieldStyle())
          .padding(.horizontal)

        // MARK: - Fetch Button
        Button("Get Recommendations") {
          Task { await vm.fetchRecommendations() }
        }
        .buttonStyle(.borderedProminent)

        // MARK: - Loading Indicator
        if vm.isLoading {
          ProgressView()
            .padding()
        }

        // MARK: - Error Message
        if let error = vm.errorMessage {
          Text(error)
            .foregroundColor(.red)
            .padding(.horizontal)
        }

        // MARK: - Recommendations List
        ScrollView {
          VStack(alignment: .leading, spacing: 12) {
            ForEach(vm.recommendations) { item in
              VStack(alignment: .leading, spacing: 4) {
                Text(item.title)
                  .font(.headline)
                if let overview = item.overview {
                  Text(overview)
                    .font(.subheadline)
                    .foregroundColor(.gray)
                    .lineLimit(2)
                }
                if let reason = item.matchReason {
                  Text("Match Reason: \(reason)")
                    .font(.caption)
                    .foregroundColor(.secondary)
                }
              }
              .padding(.horizontal)
            }
          }
        }
      }
      .navigationTitle("Recommendations")
    }
  }
}

struct RecommendationsView_Previews: PreviewProvider {
  static var previews: some View {
    RecommendationsView()
  }
}
