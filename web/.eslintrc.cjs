module.exports = {
	root: true,
	env: { browser: true, es2020: true },
	extends: [
		'eslint:recommended',
		'plugin:@typescript-eslint/strict-type-checked',
		'plugin:@typescript-eslint/stylistic-type-checked',
		'plugin:react-hooks/recommended',
		'plugin:react/recommended',
		'plugin:react/jsx-runtime',
	],
	ignorePatterns: ['dist', '.eslintrc.cjs'],
	parser: '@typescript-eslint/parser',
	plugins: ['react-refresh'],
	rules: {
		'react-refresh/only-export-components': ['warn', { allowConstantExport: true }],
		'@typescript-eslint/no-confusing-void-expression': 'off',
		'@typescript-eslint/restrict-template-expressions': [
			'error',
			{ allowNumber: true, allowBoolean: true, allowAny: true, allowNullish: true },
		],
		'@typescript-eslint/only-throw-error': 'off',
	},
	parserOptions: {
		ecmaVersion: 'latest',
		sourceType: 'module',
		project: ['./tsconfig.json', './tsconfig.app.json', './tsconfig.node.json'],
		tsconfigRootDir: __dirname,
	},
}
