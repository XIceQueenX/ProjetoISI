//
//  AddOrEditMovieView.swift
//  TrabalhoISI
//
//  Created by Glória Martins on 27/12/2025.
//

import SwiftUI

struct AddOrEditMovieView: View {
  @Environment(\.dismiss) var dismiss
  @ObservedObject var vm: MoviesViewModel
  @State var movie: Movie
  
  var body: some View {
    NavigationStack {
      Form {
        Section("Details") {
          TextField("Title", text: $movie.title)
          TextField("Original Title", text: Binding(
            get: { movie.originalTitle ?? "" },
            set: { movie.originalTitle = $0 }
          ))
          
          TextField("Overview", text: Binding(
            get: { movie.overview ?? "" },
            set: { movie.overview = $0 }
          ))
          
          TextField("Release Date", text: Binding(
            get: { movie.releaseDate ?? "" },
            set: { movie.releaseDate = $0 }
          ))
          
        }
        
        if movie.id != 0 { // existing movie
          Button(role: .destructive) {
            Task {
              await vm.deleteMovie(movie: movie)
              dismiss()
            }
          } label: {
            Text("Delete Movie")
          }
        }
      }
      .navigationTitle(movie.id == 0 ? "Add Movie" : "Edit Movie")
      .toolbar {
        ToolbarItem(placement: .confirmationAction) {
          Button("Save") {
            Task {
              await vm.addOrEditMovie(movie: movie)
              dismiss()
            }
          }
        }
        ToolbarItem(placement: .cancellationAction) {
          Button("Cancel") { dismiss() }
        }
      }
    }
  }
}

// Helper for optional bindings
extension Binding where Value == String? {
  init(_ source: Binding<String?>, replacingNilWith defaultValue: String) {
    self.init(get: { source.wrappedValue ?? defaultValue },
              set: { source.wrappedValue = $0 })
  }
}
