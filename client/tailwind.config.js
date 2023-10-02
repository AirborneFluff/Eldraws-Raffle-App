/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    fontFamily: {
      sans: ['Open Sans', 'Helvetica', 'Arial', 'sans-serif'],
      display: ['Poppins', 'Helvetica', 'Arial', 'sans-serif'],
    },
    extend: {
      colors: {
        primary: {
          '0': '#efecf9',
          '50': '#dfd8f3',
          '100': '#cfc5ed',
          '150': '#bfb1e7',
          '200': '#af9ee2',
          '250': '#9f8bdc',
          '300': '#8f77d6',
          '350': '#7f64d0',
          '400': '#6f50ca',
          '450': '#6f50ca',
          '500': '#5f3dc4',
          '550': '#5637b0',
          '600': '#4c319d',
          '650': '#432b89',
          '700': '#392576',
          '750': '#301f62',
          '800': '#26184e',
          '850': '#1c123b',
          '900': '#130c27',
          '950': '#090614',
          '1000': '#000000'
        },
      },
    },
  },
  plugins: [],
}

